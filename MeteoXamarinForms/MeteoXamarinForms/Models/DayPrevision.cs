using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoXamarinForms.Models
{
    public class DayPrevision
    {
        public DateTime DaysOfWeek { get; set; }
        public string ProbabilityIcon { get; set; }
        public int ProbalilityOfPrecipitation { get; set; }
        public string DayIcon { get; set; }
        public int MinTemperature { get; set; }
        public int MaxTemperature { get; set; }

    }
}
