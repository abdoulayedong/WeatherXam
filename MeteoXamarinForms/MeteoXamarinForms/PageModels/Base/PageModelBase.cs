using FreshMvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MeteoXamarinForms.ViewModels.Base
{
    public class PageModelBase : FreshBasePageModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //private string _title;

        //public string Title
        //{
        //    get { return _title; }
        //    set => SetProperty(ref _title, value);
        //}

        //private bool _isLoading;


        //public bool IsLoading
        //{
        //    get { return _isLoading; }
        //    set { SetProperty(ref _isLoading, value); }
        //}

        //public virtual Task InitializeAsync(object navigationData = null)
        //{
        //    return Task.CompletedTask;
        //}

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if(EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
