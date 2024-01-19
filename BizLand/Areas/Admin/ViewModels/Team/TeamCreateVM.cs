

using BizLand.Models;
using System.ComponentModel.DataAnnotations;

namespace BizLand.Areas.Admin.ViewModels.Team
{
    public class TeamCreateVM
    {
        [Required(ErrorMessage="Fullname is required")]
        [MinLength(6,ErrorMessage ="Fullname can contain minimum 6 characters")]
        [MaxLength(50,ErrorMessage ="Fullname can contain maximum 50 characters")]
        public string FullName { get; set; }
        [Required(ErrorMessage ="Photo is required")]
        public IFormFile Photo { get; set; }
        public string? ImageURL { get; set; }
        public string FaceLink { get; set; }
        public string TwitLink { get; set; }
        public string InstaLink { get; set; }
        public string LinkedInLink { get; set; }

        public int? PositionId { get; set; }
        public List<Position>? Positions { get; set; }
    }
}
