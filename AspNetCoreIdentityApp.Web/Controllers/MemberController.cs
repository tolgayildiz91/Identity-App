using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Repository.Models;
using AspNetCoreIdentityApp.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Security.Claims;
using AspNetCoreIdentityApp.Core.Models;
using AspNetCoreIdentityApp.Service.Services;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;
        private string userName => User.Identity!.Name!;
        private readonly IMemberService _memberService;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider, IMemberService memberService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
            _memberService = memberService;
        }

        public async Task<IActionResult> Index()
        {

            var userClaims = User.Claims.ToList();
            var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            return View(await _memberService.GetUserViewModelByUserNameAsync(userName) );
        }
        public async Task Logout()
        {
           await _memberService.LogoutAsync();
        }
        public IActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(!await _memberService.CheckPasswordAsync(userName,request.PasswordOld))
            {
                ModelState.AddModelError(string.Empty, "Eski Şifreniz Yanlıştır");
                return View();
            }
            var (isSuccess,errors) = await _memberService.ChangePasswordAsync(userName, request.PasswordOld, request.PasswordNew);
            if(!isSuccess)
            {
                ModelState.AddModelErrorList(errors);
                return View();
            }
            TempData["SuccessMessage"] = "Şifre Başarıyla Değiştirilmiştir.";
            return View();
        }
        public async Task<IActionResult> UserEdit()
        {
            ViewBag.genderList = _memberService.GetGenderSelectList();

            return View(await _memberService.GetUserEditViewModelAsync(userName));
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var (isSuccess,errors)= await _memberService.EditUserAsync(request, userName);

            if (!isSuccess)
            {
                ModelState.AddModelErrorList(errors);
                return View();
            }
            TempData["SuccessMessage"] = "Üye Bilgileri Başarıyla Değiştirilmiştir.";

            return View(await _memberService.GetUserEditViewModelAsync(userName));
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            string message = string.Empty;

            message = "Bu Sayfayı Görmeye Yetkiniz Yoktur. Yetki Almak İçin Yöneticiniz İle Görüşebilirsiniz.";
            ViewBag.message = message;

            return View();
        }

        [HttpGet]
        public IActionResult Claims()
        {
            return View(_memberService.GetClaims(User));
        }

        [HttpGet]
        [Authorize(Policy = "AnkaraPolicy")]
        public IActionResult AnkaraPage()
        {

            return View();
        }


        [HttpGet]
        [Authorize(Policy = "ExchangePolicy")]
        public IActionResult ExchangePage()
        {

            return View();
        }

        [HttpGet]
        [Authorize(Policy = "ViolencePolicy")]
        public IActionResult ViolencePage()
        {

            return View();
        }

    }
}
