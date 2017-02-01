using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using BLL.Interface.Services;
using UI.Mvc.Models;
using BLL.Interface.Entities;
using Microsoft.AspNet.Identity;
using UI.Mvc.Infrastructure.Mappers;
using System.Threading.Tasks;
using BLL.Interface;
using System.Configuration;

namespace UI.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IPasswordsHasher passwordService;
        private readonly IEmailSender mailSender;
        private readonly IRoleService roleService;
        public AccountController(IUserService userSvc, IPasswordsHasher passHasher, IEmailSender emailSender, IRoleService roleSvc)
        {
            userService = userSvc;
            passwordService = passHasher;
            mailSender = emailSender;
            roleService = roleSvc;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterModel regInfo)
        {
            if (ModelState.IsValid)
            {
                var bllUser = regInfo.ToBllUser();
                bllUser.Password = passwordService.HashPassword(regInfo.Password);
                bllUser.IsEmailConfirmed = false;
                var role = await roleService.FindByNameAsync("unverified");
                bllUser.Roles = new List<BllRole>();
                bllUser.Roles.Add(role);
                bllUser.ActivationToken = Guid.NewGuid().ToString();
                mailSender.Username = ConfigurationManager.AppSettings["mailUsername"];
                mailSender.Password = ConfigurationManager.AppSettings["mailPassword"];
                mailSender.IsHtml = true;
                string host = Request.Url.Host;
                mailSender.Subject = "Auction. Account activation.";
                mailSender.Body = $"Your activation link: <br /> <a href='http://{host}/Account/Activate/?token={bllUser.ActivationToken}'>Activate email address</a><br>Activation token:{bllUser.ActivationToken}";
                mailSender.ToEmail = bllUser.Email;
                int id = await userService.Register(bllUser);
                if (id == (int)UserVerificationResult.ExistWithSameLogin) {
                    ModelState.AddModelError("UserName", "User with this login already exists.");
                    return View();
                }

                if (id == (int)UserVerificationResult.ExistWithSameEmail)
                {
                    ModelState.AddModelError("Email", "User with this email already exists");
                    return View();
                }
                await mailSender.Send();
                TempData["CheckInbox"] = "Check your email and confirm it.";
                return RedirectToAction("Activate", "Account");
            }

            return View(regInfo);
        }

        [HttpGet]
        public async Task<ActionResult> Activate(string token)
        {
            object checkMsg;
            if (string.IsNullOrEmpty(token))
                if (!TempData.TryGetValue("CheckInbox", out checkMsg))
                    return RedirectToAction("Index", "Home");
                else
                {
                    ViewData["Confirmed"] = checkMsg.ToString();
                    return View();
                }
                    

            if (await userService.VerifyEmail(token))
            {
                ViewData["Confirmed"] = "Email successfully confirmed.";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string login, string password)
        {
            var bllUser = await userService.Authenticate(login, password);
            if (bllUser != null)
            {
                //var identity = userService.CreateIdentity(bllUser, true);
                var principal = await userService.Authorize(bllUser);
                IEnumerable<Claim> roles = bllUser.Roles.Select(t => t.ToClaim());
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(principal.Identity, roles);
                Request.GetOwinContext().Authentication.SignIn(claimsIdentity);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        
    }
}