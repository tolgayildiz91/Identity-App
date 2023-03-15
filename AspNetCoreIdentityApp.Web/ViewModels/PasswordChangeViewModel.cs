using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class PasswordChangeViewModel
    {

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Alanı Boş Bırakılamaz.")]
        [Display(Name = "Eski Şifre :")]
        [MinLength(6,ErrorMessage ="Şifreniz en az 6 karakter olabilir")]
        public string PasswordOld { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Alanı Boş Bırakılamaz.")]
        [Display(Name = "Yeni Şifre :")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string PasswordNew { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(PasswordNew), ErrorMessage = ("Girmiş olduğunuz şifreler uyumsuzdur."))]
        [Required(ErrorMessage = "Yeni Şifre Tekrar Alanı Boş Bırakılamaz.")]
        [Display(Name = "Yeni ŞifreŞifre Tekrar :")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string PasswordConfirm { get; set; }

    }
}
