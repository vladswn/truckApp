﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AppTrucking.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace AppTrucking.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(/*ManageMessageId? message*/)
        {
            //ViewBag.StatusMessage =
            //    message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
            //    : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
            //    : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
            //    : message == ManageMessageId.Error ? "An error has occurred."
            //    : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
            //    : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
            //    : "";

            //var userId = User.Identity.GetUserId();
            //var model = new IndexViewModel
            //{
            //    HasPassword = HasPassword(),
            //    PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
            //    TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
            //    Logins = await UserManager.GetLoginsAsync(userId),
            //    BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            //};
            //return View(model);
            var getUserId = IdentityExtensions.GetUserId(User.Identity);
            ApplicationDbContext context = new ApplicationDbContext();
            //var get = context.Users.Where(s => s.Id == getUserId).FirstOrDefault();
            //return View(get);
            var user = await UserManager.FindByIdAsync(getUserId);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            EditUserViewModel nd = new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                CompanyName = user.CompanyName,
                Skype = user.Skype,
                Surname = user.Surname,
                Telephone = user.Telephone,
                Viber = user.Viber
            };

            return View(nd);
        }
        [HttpPost]
        public ActionResult Index(EditUserViewModel editUser)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            if (ModelState.IsValid)
            {
                //var user = await UserManager.FindByIdAsync(editUser.Id);
                var user = db.Users.Find(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.Email = editUser.Email;
                user.Name = editUser.Name;
                user.Surname = editUser.Surname;
                user.CompanyName = editUser.CompanyName;
                user.Skype = editUser.Skype;
                user.Telephone = editUser.Telephone;
                user.Viber = editUser.Viber;

                //db.Users.Attach(user);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }
        //[HttpPost]
        //public ActionResult Index(EditUserViewModel editUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = UserManager.FindByIdAsync(editUser.Id);
        //        if (user == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        user. .UserName = editUser.Use;
        //        user.Email = editUser.Email;
        //        user.Name = editUser.Name;
        //        user.Surname = editUser.Surname;
        //        user.Email = editUser.Email;
        //        user.CompanyName = editUser.CompanyName;
        //        user.Skype = editUser.Skype;
        //        user.Telephone = editUser.Telephone;
        //        user.Viber = editUser.Viber;


        //        return RedirectToAction("Index");
        //    }
        //    ModelState.AddModelError("", "Something failed.");
        //    return View();
        //}

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }
        [HttpGet]
        public ActionResult MyOrders()
        {
            var getUserId = IdentityExtensions.GetUserId(User.Identity);
            List<OrderViewModels> orders;
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                orders = (from or in context.Orders
                          join mp in context.MapDatas on or.OrderId equals mp.OrderId
                          join us in context.Users on or.ApplicationUserId equals us.Id
                          join cr in context.Cars on or.CarId equals cr.CarId
                          join dr in context.Drivers on or.CarId equals dr.CarId
                          //where (us.Id == Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(User.Identity))
                          where(getUserId == us.Id)
                          select new OrderViewModels()
                          {
                                #region Feilds
                              OrderId = or.OrderId,
                              BodyVolume = cr.BodyVolume,
                              CompanyName = us.CompanyName,
                              Description = or.Description,
                              Distance = mp.Distance,
                              Duration = mp.Duration,
                              E_mail = us.Email,
                              From = mp.From,
                              LiftingCapacity = cr.LiftingCapacity,
                              Name = us.Name,
                              OrderTime = or.OrderTime,
                              Prce = cr.Prce,
                              Skype = us.Skype,
                              Status = or.Status,
                              Surname = us.Surname,
                              Telephone = us.Telephone,
                              Title = cr.Title,
                              To = mp.To,
                              Tonnage = cr.Tonnage,
                              Total = or.Total,
                              Viber = us.Viber,
                              UserId = us.Id,
                              DriverName = dr.Name,
                              DriverSurName = dr.Surname,
                              DriverPhone = dr.Telephone,
                              Volume = or.Volume,
                              Weight = or.Weight,
                              CarNumber = cr.Number,
                              OrderDate = or.OrderDate,
                              Services = or.Services.ToList()
                              #endregion
                          }).Where(s=> s.Status == true).ToList();

                return View(orders);
            }

        }

        [HttpGet]
        public ActionResult PastOrder()
        {
            var getUserId = IdentityExtensions.GetUserId(User.Identity);
            List<OrderViewModels> orders;
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                orders = (from or in context.Orders
                          join mp in context.MapDatas on or.OrderId equals mp.OrderId
                          join us in context.Users on or.ApplicationUserId equals us.Id
                          join cr in context.Cars on or.CarId equals cr.CarId
                          join dr in context.Drivers on or.CarId equals dr.CarId
                          //where (us.Id == Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(User.Identity))
                          where (getUserId == us.Id)
                          select new OrderViewModels()
                          {
                              #region Feilds
                              OrderId = or.OrderId,
                              BodyVolume = cr.BodyVolume,
                              CompanyName = us.CompanyName,
                              Description = or.Description,
                              Distance = mp.Distance,
                              Duration = mp.Duration,
                              E_mail = us.Email,
                              From = mp.From,
                              LiftingCapacity = cr.LiftingCapacity,
                              Name = us.Name,
                              OrderTime = or.OrderTime,
                              Prce = cr.Prce,
                              Skype = us.Skype,
                              Status = or.Status,
                              Surname = us.Surname,
                              Telephone = us.Telephone,
                              Title = cr.Title,
                              To = mp.To,
                              Tonnage = cr.Tonnage,
                              Total = or.Total,
                              Viber = us.Viber,
                              UserId = us.Id,
                              DriverName = dr.Name,
                              DriverSurName = dr.Surname,
                              DriverPhone = dr.Telephone,
                              Volume = or.Volume,
                              Weight = or.Weight,
                              CarNumber = cr.Number,
                              OrderDate = or.OrderDate,
                              Services = or.Services.ToList()
                              #endregion
                          }).Where(s => s.Status == false).ToList();

                return View(orders);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }


#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}