using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SemesterProject.IdentityServer.Entities;
using SemesterProject.IdentityServer.Models;


namespace SemesterProject.IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IEmailSender _emailSender;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IIdentityServerInteractionService interactionService, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interactionService = interactionService;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Login, loginViewModel.Password, false, false);
                if (result.Succeeded)
                {
                    var x = User.Claims.FirstOrDefault(x => x.Type == "LastName");
                    return Redirect(loginViewModel.ReturnUrl);
                }
            }
            return View(new LoginViewModel { ReturnUrl = loginViewModel.ReturnUrl });
        }
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

            if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = registerViewModel.Login,
                Email = registerViewModel.Email,
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                List<Claim> claims = new List<Claim> {
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName)
                };
                await _userManager.AddClaimsAsync(user, claims);
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Auth",
                        values: new { userId = user.Id, code, redirectUrl = registerViewModel.ReturnUrl },
                        protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(registerViewModel.Email, "MyFace confirm email",
                       $"Hi {user.FirstName} {user.LastName}, Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


            return RedirectToAction("EmailConfirmation");
        }
        public async Task<IActionResult> ConfirmEmail(Guid userId, string code, string redirectUrl)
        {
            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(code))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Redirect(redirectUrl);
            }
            return View();
        }
        public IActionResult EmailConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword(string returnUrl)
        {
            return View(new ForgotPasswordModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction("./ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action(
                            "ResetPassword",
                            "Auth",
                            values: new { userId = user.Id, code, redirectUrl = forgotPasswordModel.ReturnUrl },
                            protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(forgotPasswordModel.Email, "MyFace reset password email",
                           $"Hi, you can reset your password <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToAction("ForgotPasswordConfirmation");
            }
            return View(forgotPasswordModel);
        }
        [HttpGet]
        public IActionResult ResetPassword(Guid userId, string code, string redirectUrl)
        {
            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(code))
            {
                return NotFound();
            }
            return View(new ResetPasswordModel { Code = code, Password = null, ConfirmPassword = null, ReturnUrl = redirectUrl, UserId = userId });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(resetPasswordModel.UserId.ToString());
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordModel.Code));
                var response = await _userManager.ResetPasswordAsync(user, code, resetPasswordModel.Password);
                if (response.Succeeded)
                {
                    return Redirect(resetPasswordModel.ReturnUrl);
                }
            }
            return RedirectToAction("Login/",new {
                returnUrl = resetPasswordModel.ReturnUrl});
        }
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
    }
}