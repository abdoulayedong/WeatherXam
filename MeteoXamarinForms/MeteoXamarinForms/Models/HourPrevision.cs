using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoXamarinForms.Models
{
    public class HourPrevision
    {
        public DateTime Hour { get; set; }
        public string Icon { get; set; }
        public int Temperature { get; set; }
        public int ProbalilityOfPrecipitation { get; set; }
        public string ProbabilityIcon { get; set; }
    }
}
