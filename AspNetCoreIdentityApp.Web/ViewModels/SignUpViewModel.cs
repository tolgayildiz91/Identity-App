using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class SignUpViewModel
    {
        public SignUpViewModel()
        {
            
        }
        public SignUpViewModel(string userName, string email, string phone, string password)
        {
            UserName = userName;
            Email = email;
            Phone = phone;
            Password = password;
        }
        [Required(ErrorMessage ="Kullanıcı Ad Alanı Boş Bırakılamaz.")]
        [Display(Name ="Kullanıcı Adı :")]
        public string? UserName { get; set; }

        [EmailAddress(ErrorMessage ="Email Formatı Yanlıştır.")]
        [Required(ErrorMessage = "Email Alanı Boş Bırakılamaz.")]
        [Display(Name = "Email :")]
        public string? Email { get; set; }


        [Required(ErrorMessage = "Telefon Alanı Boş Bırakılamaz.")]
        [Display(Name = "Telefon :")]
        public string? Phone { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Alanı Boş Bırakılamaz.")]
        [Display(Name = "Şifre :")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage =("Girmiş olduğunuz şifreler uyumsuzdur."))]
        [Required(ErrorMessage = "Şifre Tekrar Alanı Boş Bırakılamaz.")]
        [Display(Name = "Şifre Tekrar :")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string? PasswordConfirm { get; set; }


    }
}
