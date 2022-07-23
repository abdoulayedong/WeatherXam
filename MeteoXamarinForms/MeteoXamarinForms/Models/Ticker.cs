using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Timers;
using Xamarin.Essentials;

namespace MeteoXamarinForms.Models
{
    public class Ticker : INotifyPropertyChanged
    {
        public Ticker()
        {
            Timer timer = new ();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public DateTime Now
        {
            get 
            {

                return DateTime.UtcNow.AddSeconds(Preferences.Get("TimezoneOffset",0)); 
            }
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Now"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
