using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class SignInViewModel
    {

        [Required(ErrorMessage = "Email  Alanı Boş Bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Email Formatı Yanlıştır.")]
        [Display(Name = "Email :")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Alanı Boş Bırakılamaz.")]
        [Display(Name = "Şifre :")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string? Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public  bool RememberMe { get; set; }

    }
}
