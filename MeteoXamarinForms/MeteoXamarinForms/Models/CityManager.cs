using MeteoXamarinForms.ViewModels.Base;
using System;

namespace MeteoXamarinForms.Models
{
    public class CityManager //: PageModelBase
    {
        public string City { get; set; }
        public int Temperature { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Country { get; set; }
        public string Icon { get; set; }
        public bool IsLocalPosition { get; set; }
        //private bool _isBeingDragged;
        //public bool IsBeingDragged
        //{
        //    get { return _isBeingDragged; }
        //    set { SetProperty(ref _isBeingDragged, value); }
        //}

        //private bool _isBeingDraggedOver;
        //public bool IsBeingDraggedOver
        //{
        //    get { return _isBeingDraggedOver; }
        //    set { SetProperty(ref _isBeingDraggedOver, value); }
        //}
    }
}
