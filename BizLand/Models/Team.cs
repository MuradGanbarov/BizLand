namespace BizLand.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ImageURL { get; set; }
        public int? PositionId { get; set; }
        public Position? Position { get; set; }
        public string FaceLink { get; set; }
        public string TwitLink { get; set; }
        public string InstaLink{ get; set; }
        public string  LinkedInLink { get; set; }
    }
}
