using AspNetCoreIdentityApp.Repository.Models;
using AspNetCoreIdentityApp.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Service.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Security.Claims;
using AspNetCoreIdentityApp.Core.ViewModels;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailServices _emailServices;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailServices emailServices)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailServices = emailServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var identityResult = await _userManager.CreateAsync(new() { UserName = request.UserName, PhoneNumber = request.Phone, Email = request.Email }, request.PasswordConfirm!);

            if (!identityResult.Succeeded)
            {
                ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }


            var exchangeExpireClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays(10).ToString());
            var user = await _userManager.FindByNameAsync(request.UserName);
            var claimResult = await _userManager.AddClaimAsync(user, exchangeExpireClaim);

            if(!claimResult.Succeeded)
            {
                ModelState.AddModelErrorList(claimResult.Errors.Select(x => x.Description).ToList());
                return View();
            }

            TempData["SuccessMessage"] = "Üyelik Kayıt İşlemi Başarıyla Gerçekleşmiştir";
            return RedirectToAction(nameof(HomeController.SignUp));

        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {

            var hasUser = await _userManager.FindByEmailAsync(request.Email!);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu email adresine sahip kullanıcı bulunamamıştır.");
                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);
            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

            // örnek link https://localhost:7237?userId=12213&token=asdasdasdasda

            //email service
            await _emailServices.SendResetPasswordEmail(passwordResetLink!, hasUser.Email!);


            TempData["SuccessMessage"] = "Şifre Sıfırlama Linki Eposta Adresinize Gönderilmiştir.";
            return RedirectToAction(nameof(ForgetPassword));
        }


        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;





            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if (userId == null || token == null)
            {
                throw new Exception("Bir Hata Meydana Geldi");
            }

            var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı Bulunamamıştır.");
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(hasUser, token.ToString()!, request.Password);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz Başarıyla Yenilenmiştir";
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());
                return View();
            }

            return View();
        }



        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
        {

            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            var hasUser = await _userManager.FindByEmailAsync(model.Email!);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya Şifre Yanlış");
                return View();
            }



            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password!, model.RememberMe, true);

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "Hesap Kitlendi. 3 Dakika Boyunca Giriş Yapamazsınız." });
                return View();
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelErrorList(new List<string>() { $"Email veya Şifre Yanlış.", $"Başarısız Giriş Sayısı = {await _userManager.GetAccessFailedCountAsync(hasUser)}" });
                return View();
            }


            if (hasUser.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(hasUser, model.RememberMe, new[] { new Claim("birthdate", hasUser.BirthDate.Value.ToString()) });

            }
            return Redirect(returnUrl!);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}