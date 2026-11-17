using TARge25shop.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TARge25shop.ApplicationServices.Services;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Models.Account;
using RegisterViewModel = TARge25shop.Models.Account.RegisterViewModel;
using System.Diagnostics;
using TARge25shop.Models;
using System.Globalization;

namespace TARge25śhop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailServices _emailServices;


        public AccountController
            (
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                IEmailServices emailServices
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailServices = emailServices;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = vm.Email,
                    Name = vm.Name,
                    Email = vm.Email,
                    City = vm.City
                };

                var result = await _userManager.CreateAsync(user, vm.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink =
                        Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token },
                        Request.Scheme);

                    EmailTokenDto newsignup = new();
                    newsignup.Token = token;
                    newsignup.Body = $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>";
                    newsignup.Subject = "SHOP Confirm your email";
                    newsignup.To = user.Email;

                    if(_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    _emailServices.SendEmailToken(newsignup, token);
                    List<string> errordatas =
                        [
                        "Area", "Accounts",
                        "Issue", "Success",
                        "StatusMessage", "Registration Success",
                        "ActedOn", $"{vm.Email}",
                        "CreatedAccountData", $"{vm.Email}\n{vm.City}\n[password hidden]\n[password hidden]"
                        ];
                    ViewBag.ErrorData = errordatas;
                    ViewBag.ErrorTitle = "Registration Success";
                    ViewBag.ErrorMessage = "Please check your email to confirm your account.";

                    return View("ConfirmationEmailMessage");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                // Implement registration logic here
                // For example, create a new user and save it to the database
                // Redirect to a success page or login page after successful registration
                return View(vm);
            }
            // If we got this far, something failed; redisplay the form
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The user with id of {userId} is not valid";
                return View("NotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            List<string> errordatas =
                [
                "Area", "Accounts",
                "Issue", "Success",
                "StatusMessage", "Registration Success",
                "ActedOn", $"{user.Email}",
                "CreatedAccountData", $"{user.Email}\n{user.City}\n[password hidden]"
                ];

            if (result.Succeeded)
            {
                errordatas =
                [
                "Area", "Accounts",
                "Issue", "Success",
                "StatusMessage", "Registration Success",
                "ActedOn", $"{user.Email}",
                "CreatedAccountData", $"{user.Email}\n{user.City}\n[password hidden]"
                ];
                ViewBag.ErrorMessage = errordatas;
                return View();
            }

            ViewBag.ErrorDatas = errordatas;
            ViewBag.ErrorTitles = "Email cannot be confirmed";
            ViewBag.ErrorMessage = $"The user email with userid of {userId}, cannot be confirmed";
            
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> Login(string? returnUrl)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                
                if (user != null && !user.EmailConfirmed && 
                    (await _userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else {
                        return RedirectToAction("Index", "Home");
                    }
                }
                if(result.IsLockedOut)
                {
                    return View("AccountLocked");
                }

                ModelState.AddModelError("", "Invalid login Attempt");
            }

            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink =
                        Url.Action("ResetPassword", "Account",
                        new { userId = user.Id, token = token },
                        Request.Scheme);

                    var emailDto = new EmailDto
                    {
                        To = model.Email,
                        Subject = "Password reset",
                        Body =  $"Reset your password at this link <a href='{passwordResetLink}'></a>"
                    };

                    _emailServices.SendEmail(emailDto);

                    return View("ForgotPasswordConfirmation");
                }
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token) )
            {
                ModelState.AddModelError(string.Empty, "Invalid password reset token");

                
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            return View(model);
        }
    }
}