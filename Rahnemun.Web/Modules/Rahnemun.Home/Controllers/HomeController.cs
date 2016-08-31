using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Edreamer.Framework.Settings;
using Rahnemun.ContactUsContracts;
using Rahnemun.EmailContracts;
using Rahnemun.Home.Models;
using Rahnemun.HomeContracts;

namespace Rahnemun.Home.Controllers
{
    public class HomeController: Controller
    {
        private readonly ISettingsService _settingsService;
        private readonly IEnumerable<IHomeWidgetProvider> _providers;

        public HomeController(ISettingsService settingsService, IEnumerable<IHomeWidgetProvider> providers)
        {
            _settingsService = settingsService;
            _providers = providers;
        }

        // GET: /
        [HttpGet]
        public ActionResult Index()
        {
            var slides = _providers
                   .SelectMany(p => p.GetHomeSlideWidgets())
                   .OrderByDescending(w => w.Priority)
                   .ToList();
            var intros = _providers
                .SelectMany(p => p.GetHomeIntroWidgets())
                .OrderByDescending(w => w.Priority)
                .ToList();
            var widgets = _providers
                .SelectMany(p => p.GetHomeWidgets())
                .OrderByDescending(w => w.Priority)
                .ToList();
            
            return View("Home", new HomeViewModel { Slides = slides, Intros = intros, Widgets = widgets });
        }

        // GET: /howitworks
        [HttpGet]
        public ActionResult HowItWorks()
        {
            return View("HowItWorks", new HowItWorksViewModel
                                      {
                                          ContactTelNo = _settingsService.GetContactTelNo(),
                                          ContactTelTitle = _settingsService.GetContactTelTitle()
                                      });
        }

        // GET: /about
        [HttpGet]
        public ActionResult About()
        {
            return View("About");
        }

        // GET: /terms
        [HttpGet]
        public ActionResult Terms()
        {
            return View("Terms");
        }

        // GET: /privacy
        [HttpGet]
        public ActionResult Privacy()
        {
            return View("Privacy");
        }

        // GET/POST: /error?Error=...
        public ActionResult Error(string error)
        {
            Response.TrySkipIisCustomErrors = true;
            return View("Error", new ErrorViewModel
                                 {
                                     ErrorMessage = error,
                                     SupportEmail = _settingsService.GetSupportEmail(),
                                     ContactTelNo = _settingsService.GetContactTelNo(),
                                     ContactTelTitle = _settingsService.GetContactTelTitle()
                                 });
        }
    }
}
