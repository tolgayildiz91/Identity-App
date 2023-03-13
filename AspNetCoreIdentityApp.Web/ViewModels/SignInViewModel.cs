using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "Email  Alanı Boş Bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Email Formatı Yanlıştır.")]
        [Display(Name = "Email :")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Şifre Alanı Boş Bırakılamaz.")]
        [Display(Name = "Şifre :")]
        public string? Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public  bool RememberMe { get; set; }

    }
}
