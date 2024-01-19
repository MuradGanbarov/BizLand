using System.ComponentModel.DataAnnotations;

namespace BizLand.Areas.Admin.ViewModels.Service
{
    public class ServiceUpdateVM
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(3, ErrorMessage = "Name can contain minimum 3 characters")]
        [MaxLength(25, ErrorMessage = "Name can contain maximum 25 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MinLength(5, ErrorMessage = "Description can contain minimum 5 characters")]
        [MaxLength(300, ErrorMessage = "Name can contain maximum 300 characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Icon is required")]
        public string Icon { get; set; }
    }
}
