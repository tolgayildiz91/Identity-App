using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Models
{
    public class RoleCreateViewModel
    {
        [Required(ErrorMessage = "Rol Adı Boş Bırakılamaz.")]
        [Display(Name = "Rol Adı :")]
        public string Name { get; set; }
    }
}
