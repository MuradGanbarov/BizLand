using BizLand.Models;

namespace BizLand.ViewModels
{
    public class HomeVM
    {
        public List<Position>? Positions { get; set; }
        public List<Service>? Services { get; set; }
        public List<Team>? Teams { get; set; }
    }
}
