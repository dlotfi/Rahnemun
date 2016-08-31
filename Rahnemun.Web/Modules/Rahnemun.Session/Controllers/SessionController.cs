using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Edreamer.Framework.Context;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Injection;
using Edreamer.Framework.Media;
using Edreamer.Framework.Mvc;
using Edreamer.Framework.Mvc.Extensions;
using Edreamer.Framework.Security;
using Edreamer.Framework.UI.Notification;
using Rahnemun.CategoryContracts;
using Rahnemun.Common;
using Rahnemun.PaymentContracts;
using Rahnemun.Session.Models;
using Rahnemun.Session.Services;
using Rahnemun.SessionContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.Session.Controllers
{
    public class SessionController : Controller
    {
        private readonly IConsulteeService _consulteeService;
        private readonly ISessionService _sessionService;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IMediaService _mediaService;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IDateTimeLocalizationService _dateTimeLocalizationService;
        private readonly IConsultantService _consultantService;
        private readonly ICategoryService _categoryService;
        private readonly INotifier _notifier;
        private readonly IPaymentService _paymentService;

        public SessionController(IConsulteeService consulteeService, IConsultantService consultantService, ICategoryService categoryService,
                                 INotifier notifier, IPaymentService paymentService, ISessionService sessionService,
                                 IUserService userService, IMessageService messageService, IMediaService mediaService,
                                 IWorkContextAccessor workContextAccessor, IDateTimeLocalizationService dateTimeLocalizationService)
        {
            _consulteeService = consulteeService;
            _sessionService = sessionService;
            _userService = userService;
            _messageService = messageService;
            _mediaService = mediaService;
            _workContextAccessor = workContextAccessor;
            _dateTimeLocalizationService = dateTimeLocalizationService;
            _consultantService = consultantService;
            _categoryService = categoryService;
            _notifier = notifier;
            _paymentService = paymentService;
        }

        // GET: /session/{Id}
        [HttpGet]
        public ActionResult Conversation(int id)
        {
            var session = _sessionService.GetSession(id);
            if (session == null) return HttpNotFound();

            // Authorize
            var currentUser = _workContextAccessor.Context.CurrentUser();
            if (currentUser == null)
                return new HttpUnauthorizedResult("به منظور دسترسی به جلسه‌های مشاوره ورود شما به رهنمون الزامی است.");
            var userParticipationType = GetCurrentUserParticipationType(session, currentUser);
            if (userParticipationType == SessionParticipationType.Other)
                return new HttpUnauthorizedResult("شما اجازه دسترسی به این جلسه را ندارید.");
            
            var viewModel = Injector.Flat(new SessionViewModel
                                          {
                                              ConsulteeFullName = _userService.GetUserFullName(session.Consultee),
                                              ConsultantFullName = _userService.GetUserFullName(session.Consultant),
                                              SourceUrl = Url.Action("GetMessages", new { Id = id }),
                                              SendUrl = Url.Action("SendMessage", new { Id = id }),
                                              SetSeenUrl = Url.Action("SetSeen", new { Id = id }),
                                              StopUrl = Url.Action("Stop", new { Id = id }),
                                              NewSessionUrl = Url.Route<IStartNewSessionRoute>()
                                                                 .Get(new StartNewSessionRouteModel { CategoryId = session.Category.Id, ConsultantId = session.Consultant.Id }),
                                              UserParticipationType = userParticipationType,
                                              Stopped = session.StopTime != null
                                          }, session);
            return View("Conversation", viewModel);
        }

        // GET: /message/{Id}/attachment
        [HttpGet]
        public ActionResult GetMessageAttachment(int id)
        {
            var message = _messageService.GetMessage(id);
            if (message?.MediaId == null) return HttpNotFound();

            // Authorize 
            var currentUser = _workContextAccessor.Context.CurrentUser();
            if (currentUser == null)
                return new HttpUnauthorizedResult("به منظور دسترسی به فایل‌های ضمیمه ورود شما به رهنمون الزامی است.");
            var userParticipationType = GetCurrentUserParticipationType(message.Session, currentUser);
            if (userParticipationType == SessionParticipationType.Other)
                return new HttpUnauthorizedResult("شما اجازه دسترسی به فایل‌های ضمیمه این جلسه را ندارید.");

            var media = _mediaService.GetMedia((int)message.MediaId);
            return File(media.Data, media.Type);
        }


        // GET: /session/new-session?ConsultantId={ConsultantId}&CategoryId={CategoryId}
        [HttpGet]
        public ActionResult StartNewSession(int consultantId, int categoryId)
        {
            var currentUser = _workContextAccessor.Context.CurrentUser();
            if (currentUser == null) // User has not logged-in
            {
                return Redirect(Url.Route<IConsulteeRegisterOrLoginRoute>().Get(Request.RawUrl));
            }

            var consultee = _consulteeService.GetConsultee(currentUser.Id);
            if (consultee == null) // Logged-in user is not a consultee
            {
                _notifier.Error("مشاور عزیز! با حساب کاربری مشاور امکان دریافت خدمات مشاوره وجود ندارد. به منظور دریافت خدمات مشاوره با کاربر غیرمشاور وارد سایت شوید و در صورت نداشتن حساب کاربری غیر مشاور، حساب کاربری جدید ایجاد نمایید.");
                return Redirect(Url.Route<IConsulteeRegisterOrLoginRoute>().Get(Request.RawUrl));
            }

            var consultant = _consultantService.GetConsultant(consultantId);
            if (consultant == null || !consultant.Approved) return HttpNotFound();

            var category = _categoryService.GetCategory(categoryId);
            if (category == null) return HttpNotFound();

            // If there is an avtive session between current user and chosen consultant in the chosen category redirect to that session
            var activeSession = _sessionService.Sessions.SingleOrDefault(s => s.Consultee.Id == currentUser.Id && s.Consultant.Id == consultantId && s.Category.Id == categoryId && s.StopTime == null);
            if (activeSession != null)
            {
                _notifier.Information("{0} عزیز! جلسه مشاوره شما با {1} در گروه {2} خاتمه نیافته است. چنانچه تمایل دارید جلسه جدیدی را ایجاد نمایید لطفا ابتدا این جلسه را خاتمه دهید."
                    .FormatWith(_userService.GetUserFullName(consultee), _userService.GetUserFullName(consultant), category.Caption));
                return Redirect(Url.Route<ISessionRoute>().Get(activeSession.Id));
            }

            return View("StartNewSession", new NewSessionViewModel
                                           {
                                               ConsulteeFullName = _userService.GetUserFullName(consultee),
                                               ConsulteeEmail = currentUser.Email,
                                               IsEmailConfirmed = currentUser.EmailConfirmed,
                                               ConsultantId = consultant.Id,
                                               ConsultantFullName = _userService.GetUserFullName(consultant),
                                               SessionFee = consultant.Fee,
                                               SessionFeeWithoutDiscount = GetFeeWithoutDiscount(consultant.Fee),
                                               CategoryId = category.Id,
                                               CategoryCaption = category.Caption,
                                               CategoryTerms = category.Terms
                                           });
        }

        // POST: /session/new-session?ConsultantId={ConsultantId}&CategoryId={CategoryId}
        [HttpPost]
        public ActionResult StartNewSession(int consultantId, int categoryId, bool disclaimer, string paymentProviderName)
        {
            // Authorize
            var currentUser = _workContextAccessor.Context.CurrentUser();
            if (currentUser == null)
                return new HttpUnauthorizedResult("به منظور شروع جلسه مشاوره ورود شما به رهنمون الزامی است.");
            if (_consultantService.IsConsultant(currentUser.Id))
                return new HttpUnauthorizedResult("مشاور عزیز! به منظور دریافت خدمات مشاوره با کاربر غیرمشاور وارد سایت شوید و در صورت نداشتن حساب کاربری غیر مشاور، حساب کاربری جدید ایجاد نمایید.");

            var consultant = _consultantService.GetConsultant(consultantId);
            Throw.If(consultant == null || !consultant.Approved).AnArgumentException("No approved consultant with the id {0} exists.".FormatWith(consultantId), "consultantId");

            var category = _categoryService.GetCategory(categoryId);
            Throw.IfNull(category).AnArgumentException("No category with the id {0} exists.".FormatWith(categoryId), "categoryId");

            //Todo [25081300]: Validate consultant capacity
            //Todo [23070512]: Check consultant and category relation

            var price = consultant.Fee; // Apply discount here.

            if (disclaimer)
            {
                var handlerData = new SessionPaymentHandlerDataModel {ConsulteeId = currentUser.Id, ConsultantId = consultantId, CategoryId = categoryId};
                var paymentId = _paymentService.AddPayment(currentUser.Id, paymentProviderName, price, SessionPaymentEventHandler.Id, SerializationHelpers.Serialize(handlerData));

                return Redirect(Url.Route<IPaymentRoute>().Get(new PaymentRouteModel {PaymentId = paymentId, ProviderName = paymentProviderName}));
            }

            var consultee = _consulteeService.GetConsultee(currentUser.Id);

            ModelState.AddModelError("", "تایید متن سلب مسئولیت الزامی است.");

            return View("StartNewSession", new NewSessionViewModel
                                           {
                                               ConsulteeFullName = _userService.GetUserFullName(consultee),
                                               ConsulteeEmail = currentUser.Email,
                                               IsEmailConfirmed = currentUser.EmailConfirmed,
                                               ConsultantId = consultant.Id,
                                               ConsultantFullName = _userService.GetUserFullName(consultant),
                                               SessionFee = consultant.Fee,
                                               SessionFeeWithoutDiscount = GetFeeWithoutDiscount(consultant.Fee),
                                               CategoryId = category.Id,
                                               CategoryCaption = category.Caption,
                                               CategoryTerms = category.Terms
                                           });
        }

        #region Ajax Actions

        // GET(Ajax): /session/{Id}/messages
        [Ajax, HttpGet]
        public FormattedJsonResult GetMessages(int id, DateTime? lastMessageTime)
        {
            var session = _sessionService.GetSession(id);
            Throw.IfNull(session)
                .AnArgumentException("No sesssion with id {0} exists.".FormatWith(id), "id");

            // Authorize
            var currentUser = _workContextAccessor.Context.CurrentUser();
            Throw.If(currentUser == null)
                    .A<SecurityException>("Unauthorized to get session messages.", "شما مجوز لازم برای دریافت پیامهای جلسه را ندارید.");
            var currentUserParticipationType = GetCurrentUserParticipationType(session, currentUser);
            Throw.If(currentUserParticipationType == SessionParticipationType.Other)
                .A<SecurityException>("Unauthorized to get session messages.", "شما مجوز لازم برای دریافت پیامهای جلسه را ندارید.");

            return GetMessagesJsonResult(session, currentUserParticipationType, lastMessageTime);
        }


        // POST(Ajax): /session/{Id}/send-message
        [Ajax, HttpPost]
        public FormattedJsonResult SendMessage(int id, DateTime lastMessageTime, MessageViewModel messageViewModel)
        {
            var session = _sessionService.GetSession(id);
            Throw.IfNull(session)
                .AnArgumentException("No sesssion with id {0} exists.".FormatWith(id), "id");
            Throw.If(session.StopTime != null)
                .A<InvalidOperationException>("Cannot send message in a stopped session.");

            // Authorize
            var currentUser = _workContextAccessor.Context.CurrentUser();
            Throw.If(currentUser == null)
                .A<SecurityException>("Unauthorized to send message in session.", "شما مجوز لازم برای ارسال پیام را ندارید.");
            var currentUserParticipationType = GetCurrentUserParticipationType(session, currentUser);
            Throw.If(currentUserParticipationType == SessionParticipationType.Other)
                .A<SecurityException>("Unauthorized to send message in session.", "شما مجوز لازم برای ارسال پیام را ندارید.");

            //ToDo [23081045]: Check if session is stopped

            Media attachmentMedia = null;
            if (messageViewModel.Attachment != null)
            {
                attachmentMedia = _mediaService.CreateMedia(messageViewModel.Attachment.InputStream);
                if (!_mediaService.IsMediaAcceptable(attachmentMedia))
                    ModelState.AddModelError("Attachment", "فایل آپلود شده غیر قابل قبول است.");
            }
            if (ModelState.IsValid)
            {
                var attachmentMediaId = attachmentMedia != null
                    ? _mediaService.AddMedia(attachmentMedia).Id
                    : (int?)null;
                _messageService.SendMessage(id, currentUserParticipationType == SessionParticipationType.Consultee, messageViewModel.Text, attachmentMediaId);

                return GetMessagesJsonResult(session, currentUserParticipationType, lastMessageTime);
            }
            return new FormattedJsonResult(new { success = false, errors = ModelState.Errors() }, false);
        }

        // POST(Ajax): /session/{Id}/set-seen
        [Ajax, HttpPost]
        public FormattedJsonResult SetSeen(int id, DateTime lastMessageTime)
        {
            var session = _sessionService.GetSession(id);
            Throw.IfNull(session)
                .AnArgumentException("No sesssion with id {0} exists.".FormatWith(id), "id");

            // Authorize
            var currentUser = _workContextAccessor.Context.CurrentUser();
            Throw.If(currentUser == null)
                .A<SecurityException>("Unauthorized to set unseen messages to seen.", "شما مجوز لازم برای تغییر وضعیت پیام های جدید به پیام های مشاهده شده را ندارید.");
            var currentUserParticipationType = GetCurrentUserParticipationType(session, currentUser);
            Throw.If(currentUserParticipationType == SessionParticipationType.Other)
                .A<SecurityException>("Unauthorized to set unseen messages to seen.", "شما مجوز لازم برای تغییر وضعیت پیام های جدید به پیام های مشاهده شده را ندارید.");

            var lastSeenTime = _messageService.SetMessagesAsSeen(id, currentUserParticipationType != SessionParticipationType.Consultee, lastMessageTime);
            return new FormattedJsonResult(new { Success = true, LastSeenTime = lastSeenTime });
        }

        // POST(Ajax): /session/{Id}/stop
        [Ajax, HttpPost]
        public FormattedJsonResult Stop(int id, DateTime lastMessageTime)
        {
            var session = _sessionService.GetSession(id);
            Throw.IfNull(session)
                .AnArgumentException("No sesssion with id {0} exists.".FormatWith(id), "id");

            // Authorize
            var currentUser = _workContextAccessor.Context.CurrentUser();
            Throw.If(currentUser == null)
                .A<SecurityException>("Unauthorized to stop session.", "شما مجوز لازم برای خاتمه دادن به جلسه را ندارید.");
            var currentUserParticipationType = GetCurrentUserParticipationType(session, currentUser);
            Throw.If(currentUserParticipationType == SessionParticipationType.Other)
                .A<SecurityException>("Unauthorized to stop session.", "شما مجوز لازم برای خاتمه دادن به جلسه را ندارید.");

            //ToDo [13290819]: If user is Consultant, check the required condition to let consultant stop the session has been met

            var stopType = currentUserParticipationType == SessionParticipationType.Consultant
                ? SessionStopType.ConsultantRequest
                : SessionStopType.ConsulteeRequest;
            var stopTime = _sessionService.StopSession(id, stopType);

            session.StopTime = stopTime;
            session.StopType = stopType;
            return GetMessagesJsonResult(session, currentUserParticipationType, lastMessageTime);
        }

        #endregion

        #region WebPart Actions

        [ChildActionOnly]
        public PartialViewResult ActiveSessionsWebPart()
        {
            // Authorize
            var currentUser = _workContextAccessor.Context.CurrentUser();
            Throw.IfNull(currentUser).A<SecurityException>("Unauthorized acccess!");

            var sessions = _sessionService.Sessions.Where(s => s.StopTime == null && (s.Consultee.Id == currentUser.Id || s.Consultant.Id == currentUser.Id)).ToList();
            var viewModel = _consultantService.IsConsultant(currentUser.Id)
                ? sessions
                    .OrderBy(s => _messageService.Messages.Where(m => m.Session.Id == s.Id && m.ByConsultee && m.SeenTime == null).Min(m => (DateTime?) m.SentTime) ?? DateTime.MaxValue)
                    .Select(s => new ActiveSessionDashboardViewModel
                                 {
                                     SessionUrl = Url.Route<ISessionRoute>().Get(s.Id),
                                     CategoryCaption = s.Category.Caption,
                                     FullName = _userService.GetUserFullName(s.Consultee),
                                     ProfilePictureId = s.Consultee.ProfilePictureId,
                                     Gender = s.Consultee.Gender,
                                     ProfileUrl = null,
                                     UnreadMessagesAmount = _messageService.Messages.Count(m => m.Session.Id == s.Id && m.ByConsultee && m.SeenTime == null)
                                 })
                : sessions
                    .OrderBy(s => _messageService.Messages.Where(m => m.Session.Id == s.Id && !m.ByConsultee && m.SeenTime == null).Min(m => (DateTime?) m.SentTime) ?? DateTime.MaxValue)
                    .Select(s => new ActiveSessionDashboardViewModel
                                 {
                                     SessionUrl = Url.Route<ISessionRoute>().Get(s.Id),
                                     CategoryCaption = s.Category.Caption,
                                     FullName = _userService.GetUserFullName(s.Consultant),
                                     ProfilePictureId = s.Consultant.ProfilePictureId,
                                     Gender = s.Consultant.Gender,
                                     ProfileUrl = Url.Route<IConsultantDisplayRoute>()
                                                     .Get(new ConsultantIdModel
                                                         {
                                                             ConsultantId = s.Consultant.Id,
                                                             CategoryId = s.Category.Id
                                                         }),
                                     UnreadMessagesAmount = _messageService.Messages.Count(m => m.Session.Id == s.Id && !m.ByConsultee && m.SeenTime == null)
                                 });

            return PartialView("ActiveSessionsWebPart", viewModel);
        }

        [ChildActionOnly]
        public PartialViewResult InactiveSessionsWebPart()
        {
            // Authorize
            var currentUser = _workContextAccessor.Context.CurrentUser();
            Throw.IfNull(currentUser).A<SecurityException>("Unauthorized acccess!");

            var sessions = _sessionService.Sessions.Where(s => s.StopTime != null && (s.Consultee.Id == currentUser.Id || s.Consultant.Id == currentUser.Id)).ToList();
            var viewModel = _consultantService.IsConsultant(currentUser.Id)
                ? sessions
                    .OrderBy(s => s.StopTime)
                    .Select(s => new InactiveSessionDashboardViewModel
                                 {
                                     SessionUrl = Url.Route<ISessionRoute>().Get(s.Id),
                                     CategoryCaption = s.Category.Caption,
                                     FullName = _userService.GetUserFullName(s.Consultee),
                                     ProfilePictureId = s.Consultee.ProfilePictureId,
                                     Gender = s.Consultee.Gender,
                                     ProfileUrl = null,
                                     UnreadMessagesAmount = _messageService.Messages.Count(m => m.Session.Id == s.Id && m.ByConsultee && m.SeenTime == null),
                                     StopTime = s.StopTime == null
                                             ? null
                                             : _dateTimeLocalizationService.ConvertToUserTimeZone((DateTime) s.StopTime).ToShortDateString(),
                                     StopReason = GetStopReason((SessionStopType)s.StopType)
                    })
                : sessions
                    .OrderBy(s => _messageService.Messages.Where(m => m.Session.Id == s.Id && !m.ByConsultee && m.SeenTime == null).Min(m => (DateTime?) m.SentTime) ?? DateTime.MaxValue)
                    .Select(s => new InactiveSessionDashboardViewModel
                                 {
                                     SessionUrl = Url.Route<ISessionRoute>().Get(s.Id),
                                     CategoryCaption = s.Category.Caption,
                                     FullName = _userService.GetUserFullName(s.Consultant),
                                     ProfilePictureId = s.Consultant.ProfilePictureId,
                                     Gender = s.Consultant.Gender,
                                     ProfileUrl = Url.Route<IConsultantDisplayRoute>()
                                                     .Get(new ConsultantIdModel
                                                          {
                                                              ConsultantId = s.Consultant.Id,
                                                              CategoryId = s.Category.Id
                                                          }),
                        UnreadMessagesAmount = _messageService.Messages.Count(m => m.Session.Id == s.Id && !m.ByConsultee && m.SeenTime == null),
                                     StopTime = s.StopTime == null
                                             ? null
                                             : _dateTimeLocalizationService.ConvertToUserTimeZone((DateTime) s.StopTime).ToShortDateString(),
                                     StopReason = GetStopReason((SessionStopType)s.StopType)
                                 });

            return PartialView("InactiveSessionsWebPart", viewModel);
        }

        [ChildActionOnly]
        public PartialViewResult UserCalltoActionWebPart(int consultantId, int? categoryId)
        {
            var consultant = _consultantService.GetConsultant(consultantId);

            Throw.If(consultant == null || !consultant.Approved).AnArgumentException("No approved consultant with the id {0} exists.".FormatWith(consultantId), "consultantId");

            if (categoryId == null) return null;

            var category = _categoryService.GetCategory((int) categoryId);
            Throw.IfNull(category).AnArgumentException("No category with the id {0} exists.".FormatWith(categoryId), "categoryId");

            var currentUser = _workContextAccessor.Context.CurrentUser();

            SessionModel activeSession = null;
            UserModel visitor = null;
            UserState userstate;

            if (currentUser == null)
                userstate = UserState.Aanonymous;
            else
            {
                visitor = _userService.GetUser(currentUser.Id);
                // if visitor is null so he might be preliminary consultant
                if (visitor == null)
                    userstate = UserState.PreliminaryConsultant;
                else if (_consultantService.IsConsultant(visitor.Id)) //Todo [26080932]: Also must check for preliminary consultant
                    userstate = consultantId == visitor.Id ? UserState.ConsultantWhoViewHisProfile : UserState.Consultant;
                else
                {
                    activeSession = _sessionService.Sessions.SingleOrDefault(s => s.Consultee.Id == visitor.Id && s.Consultant.Id == consultantId && s.Category.Id == categoryId && s.StopTime == null);
                    userstate = activeSession == null ? UserState.ConsulteeWithNoActiveSession : UserState.ConsulteeWithActiveSession;
                }
            }

            var viewModel = new UserCalltoActionWebPartViewModel
                            {
                                ConsultantId = consultantId,
                                CategoryId = category.Id,
                                ActiveSessionId = activeSession?.Id,
                                ConsultantFullName = _userService.GetUserFullName(consultant),
                                VisitorFullName = currentUser == null || visitor == null ? "" : _userService.GetUserFullName(visitor),
                                CategoryCaption = category.Caption,
                                UserState = userstate
                            };

            return PartialView("UserCalltoActionWebPart", viewModel);
        }

        #endregion

        private static decimal GetFeeWithoutDiscount(decimal fee)
        {
            return (decimal)Math.Ceiling((double)fee / 1000 * 1.2) * 1000;
        }

        private FormattedJsonResult GetMessagesJsonResult(SessionModel session, SessionParticipationType currentUserParticipationType, DateTime? lastTime)
        {
            var lastMessageTime = lastTime ?? DateTime.MinValue;
            var messages = _messageService.Messages
                .Where(m => m.Session.Id == session.Id && m.SentTime > lastMessageTime)
                .OrderBy(m => m.SentTime)
                .ToList();

            var allMessages = new List<MessageItem>();
            var stopped = false;
            foreach (var message in messages)
            {
                if (message.SentTime.Date != lastMessageTime.Date)
                    allMessages.Add(new MessageItem { Type = MessageType.Divider, Text = GetDateString(message.SentTime) });
                allMessages.Add(new MessageItem
                {
                    Id = message.Id,
                    Type = message.ByConsultee ? MessageType.Consultee : MessageType.Consultant,
                    Unseen = message.SeenTime == null &&
                                          (message.ByConsultee && currentUserParticipationType == SessionParticipationType.Consultant ||
                                          !message.ByConsultee && currentUserParticipationType == SessionParticipationType.Consultee),
                    SentTime = _dateTimeLocalizationService.ConvertToUserTimeZone(message.SentTime).ToShortTimeString(),
                    MediaUrl = message.MediaId == null ? null : Url.Route<IGetMessageAttachmentRoute>().Get(message.Id),
                    Text = message.Text
                });
                lastMessageTime = message.SentTime;
            }
            if (session.StopTime != null && session.StopTime > lastMessageTime)
            {
                MessageType type;
                string text;
                var time = (DateTime)session.StopTime;
                switch (session.StopType)
                {
                    case SessionStopType.ConsultantRequest:
                        type = MessageType.Consultant;
                        text = "امیدوارم این جلسه برای شما مفید بوده باشد. با توجه به اتمام زمان جلسه، بهتر است این جلسه را به پایان برسانیم. در صورت تمایل می توانیم جلسه دیگری داشته باشیم.";
                        break;
                    case SessionStopType.ConsulteeRequest:
                        type = MessageType.Consultee;
                        text = "تمایل دارم گفتگوی خود را در این جلسه به پایان برسانیم. از اینکه وقت خود را در اختیار من گذاشتید متشکرم.";
                        break;
                    case SessionStopType.ConsulteeInactivity:
                        type = MessageType.System;
                        text = $"با توجه به عدم فعالیت «{_userService.GetUserFullName(session.Consultee)}» بیش از زمان معین، طبق قوانین رهنمون این جلسه خاتمه می یابد.";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(session.StopType), session.StopType, null);
                }

                if (time.Date > lastMessageTime.Date)
                    allMessages.Add(new MessageItem { Type = MessageType.Divider, Text = GetDateString(time) });
                allMessages.Add(new MessageItem { Stop = true, Type = type, Text = text, SentTime = _dateTimeLocalizationService.ConvertToUserTimeZone(time).ToShortTimeString() });

                lastMessageTime = time;
                stopped = true;
            }

            var elapsedTime = allMessages.Any() ? _sessionService.GetSessionElapsedTime(session.Id) : (int?)null;
            return new FormattedJsonResult(new { Success = true, Messages = allMessages, LastMessageTime = lastMessageTime, ElapsedTime = elapsedTime, Stopped = stopped });
        }

        private string GetDateString(DateTime date)
        {
            var today = DateTime.UtcNow.Date;
            if (date.Date == today) return "امروز";
            if (today.Subtract(date.Date).Days == 1) return "دیروز";
            return _dateTimeLocalizationService.ConvertToUserTimeZone(date).ToShortDateString();
        }

        private static string GetStopReason(SessionStopType stopType)
        {
            switch (stopType)
            {
                case SessionStopType.ConsultantRequest: return "درخواست مشاور";
                case SessionStopType.ConsulteeRequest: return "درخواست مراجعه کننده";
                case SessionStopType.ConsulteeInactivity: return "عدم فعالیت مراجعه کننده بیش از زمان معین";
                default:
                    throw new ArgumentOutOfRangeException(nameof(stopType), stopType, null);
            }
        }

        private static SessionParticipationType GetCurrentUserParticipationType(SessionModel session, User currentUser)
        {
            if (session.Consultee.Id == currentUser.Id)
                return SessionParticipationType.Consultee;
            if (session.Consultant.Id == currentUser.Id)
                return SessionParticipationType.Consultant;
            return SessionParticipationType.Other;
        }

        private class MessageItem
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public string SentTime { get; set; }
            public string MediaUrl { get; set; }
            public MessageType Type { get; set; }
            public bool Unseen { get; set; }
            public bool Stop { get; set; }
        }

        private enum MessageType
        {
            Consultee,
            Consultant,
            System,
            Divider
        }
    }
}
