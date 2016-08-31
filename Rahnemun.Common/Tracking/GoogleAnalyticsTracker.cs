using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Edreamer.Framework.Context;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Logging;
using Edreamer.Framework.Security;
using Edreamer.Framework.Settings;

namespace Rahnemun.Common
{
    public class GoogleAnalyticsTracker: ITracker
    {
        private const string TrackingCodesSessionStateKey = "_TrackingCodes";

        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly ISettingsService _settingsService;
        private readonly HttpSessionStateBase _session;

        public GoogleAnalyticsTracker(IWorkContextAccessor workContextAccessor, ISettingsService settingsService)
        {
            _workContextAccessor = workContextAccessor;
            _settingsService = settingsService;
            _session = _workContextAccessor.Context.CurrentHttpContext().Session;
        }

        public ILogger Logger { get; set; }

        private Queue<TrackingCodeItem> TrackingCodes
        {
            get
            {
                if (_session == null) return null;
                lock (_session)
                {
                    if (_session[TrackingCodesSessionStateKey] == null)
                        _session[TrackingCodesSessionStateKey] = new Queue<TrackingCodeItem>();
                    return (Queue<TrackingCodeItem>)_session[TrackingCodesSessionStateKey];
                }
            }
        }

        public void AddDestination(string destinationPath)
        {
            Throw.IfArgumentNullOrEmpty(destinationPath, "destinationPath");
            Throw.IfNull(_session).A<InvalidOperationException>("The GoogleAnalyticsTracker requires session state to be enabled.");
            var currentUser = _workContextAccessor.Context.CurrentUser();
            TrackingCodes.Enqueue(new TrackingCodeItem
                                  {
                                      UserId = currentUser?.Id,
                                      Transaction = false,
                                      Code = $"ga('send', 'pageview', '{destinationPath}');"
                                  });
        }

        public void AddTransaction(string transactionId, string productId, string productName, string productCategory, decimal productPrice)
        {
            Throw.IfArgumentNullOrEmpty(transactionId, "transactionId");
            Throw.IfArgumentNullOrEmpty(productId, "productId");
            Throw.IfArgumentNullOrEmpty(productName, "productName");
            Throw.IfNull(_session).A<InvalidOperationException>("The GoogleAnalyticsTracker requires session state to be enabled.");
            var currentUser = _workContextAccessor.Context.CurrentUser();
            var price = (productPrice / 1000).ToString("F2", CultureInfo.InvariantCulture);
            transactionId = "tga-" + transactionId;
            productId = "pga-" + productId;
            var categoryNameValue = String.IsNullOrEmpty(productCategory) ? "" : $", 'category': '{productCategory}'";
            TrackingCodes.Enqueue(new TrackingCodeItem
            {
                UserId = currentUser?.Id,
                Transaction = true,
                Code = $"ga('ecommerce:addTransaction', {{ 'id': '{transactionId}', 'affiliation': 'Rahnemun', 'revenue': '{price}' }});" + "\r\n" +
                       $"ga('ecommerce:addItem', {{ 'id': '{transactionId}', 'name': '{productName}', 'sku': '{productId}', 'price': '{price}', 'quantity': '1' {categoryNameValue} }});" + "\r\n" +
                       "ga('ecommerce:send');"
            });
        }

        public string GetTrackingCode()
        {
            string trackingId;
            string userIdDimensionIndex;
            if (!_settingsService.TryGetSetting(new SettingEntryKey { Category = "GoogleAnalytics", Name = "TrackingId" }, out trackingId))
                return "";
            _settingsService.TryGetSetting(new SettingEntryKey { Category = "GoogleAnalytics", Name = "UserIdDimensionIndex" }, out userIdDimensionIndex);

            var trackingCode = new StringBuilder();
            var currentUser = _workContextAccessor.Context.CurrentUser();

            trackingCode.AppendLine("<script>");
            trackingCode.AppendLine("   (function(i, s, o, g, r, a, m){");
            trackingCode.AppendLine("   i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function(){");
            trackingCode.AppendLine("   (i[r].q = i[r].q ||[]).push(arguments)},i[r].l = 1 * new Date(); a = s.createElement(o),");
            trackingCode.AppendLine("   m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m);");
            trackingCode.AppendLine("   })(window, document,'script','//www.google-analytics.com/analytics.js','ga');");
            if (currentUser != null)
            {
                var userId = "uga-" + currentUser.Id;
                trackingCode.AppendLine($"   ga('create', '{trackingId}', {{ 'siteSpeedSampleRate': 100, 'userId': '{userId}' }});");
                if (!String.IsNullOrEmpty(userIdDimensionIndex))
                    trackingCode.AppendLine($"   ga('set', 'dimension{userIdDimensionIndex}', '{userId}');");
            }
            else
            {
                trackingCode.AppendLine($"   ga('create', '{trackingId}', {{ 'siteSpeedSampleRate': 100 }});");
            }    
            trackingCode.AppendLine("   ga('send', 'pageview');");

            // Other tracking codes
            lock (TrackingCodes)
            {
                var ecommerceAdded = false;
                while (TrackingCodes.Any())
                {
                    var tc = TrackingCodes.Dequeue();
                    if (tc.UserId != currentUser?.Id)
                    {
                        var user = tc.UserId == null ? "unauthenticated user" : ("user " + tc.UserId);
                        var current = currentUser == null ? "unauthenticated" : ("user " + currentUser.Id);
                        Logger.Warning($"GoogleAnalyticsTracker ignores a tracking item; because it belongs to {user} but current user is {current}.");
                        continue;
                    }
                    if (tc.Transaction && !ecommerceAdded)
                    {
                        trackingCode.AppendLine("ga('require', 'ecommerce');");
                        ecommerceAdded = true;
                    }
                    trackingCode.AppendLine(tc.Code);
                }
            }
            
            trackingCode.AppendLine("</script>");

            return trackingCode.ToString();
        }

        private class TrackingCodeItem
        {
            public int? UserId { get; set; }
            public bool Transaction { get; set; }
            public string Code { get; set; }
        }
    }
}