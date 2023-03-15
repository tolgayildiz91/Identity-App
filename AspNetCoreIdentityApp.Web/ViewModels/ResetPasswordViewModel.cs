using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class ResetPasswordViewModel
    {

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Alanı Boş Bırakılamaz.")]
        [Display(Name = "Yeni Şifre :")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = ("Girmiş olduğunuz şifreler uyumsuzdur."))]
        [Required(ErrorMessage = "Şifre Tekrar Alanı Boş Bırakılamaz.")]
        [Display(Name = "Yeni Şifre Tekrar :")]
        public string PasswordConfirm { get; set; }
    }
}
