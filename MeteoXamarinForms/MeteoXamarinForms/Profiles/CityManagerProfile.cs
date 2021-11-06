using AutoMapper;
using MeteoXamarinForms.Extensions;
using MeteoXamarinForms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoXamarinForms.Profiles
{
    public class CityManagerProfile : Profile
    {
        public CityManagerProfile()
        {
            _ = CreateMap<Root, CityManager>()
                .ForMember(dest =>
                        dest.City,
                        opt => opt.MapFrom(src => ToolExtension.GetCityName(src.Timezone)))
                    .ForMember(dest =>
                        dest.Temperature,
                        opt => opt.MapFrom(src => ToolExtension.RoundedTemperature(src.Current.Temp)))
                    .ForMember(dest =>
                        dest.Description,
                        opt => opt.MapFrom(src => src.Current.Weather[0].Description))
                    .ForMember(dest =>
                        dest.Date,
                        opt => opt.MapFrom(src => ToolExtension.GetDateTimeFromTimezone(src.Timezone_Offset)))
                    .ForMember(dest =>
                        dest.Icon,
                        opt => opt.MapFrom(src => ToolExtension.GetIcon(src.Current.Weather[0].Icon)));
        }
    }
}
