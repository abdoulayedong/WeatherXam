using MeteoXamarinForms.ViewModels.Base;
using System;

namespace MeteoXamarinForms.Models
{
    public class CityManager 
    {
        public string City { get; set; }
        public int Temperature { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Country { get; set; }
        public string Icon { get; set; }
        public bool IsLocalPosition { get; set; }
    }
}
