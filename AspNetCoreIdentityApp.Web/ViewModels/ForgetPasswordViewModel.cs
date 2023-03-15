﻿using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class ForgetPasswordViewModel
    {

        [Required(ErrorMessage = "Email  Alanı Boş Bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Email Formatı Yanlıştır.")]
        [Display(Name = "Email :")]
        public string? Email { get; set; }


    }
}
