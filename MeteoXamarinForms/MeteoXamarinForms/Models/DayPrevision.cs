using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.CommunityToolkit.Helpers;

namespace MeteoXamarinForms.Models
{
    public class DayPrevision : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private LocalizedString _daysOfWeek;

        public LocalizedString DaysOfWeek
        {
            get { return _daysOfWeek; }
            set { _daysOfWeek = value; OnPropertyChanged("DaysOfWeek"); }
        }
        public string ProbabilityIcon { get; set; }
        public int ProbalilityOfPrecipitation { get; set; }
        public string DayIcon { get; set; }
        public int MinTemperature { get; set; }
        public int MaxTemperature { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
