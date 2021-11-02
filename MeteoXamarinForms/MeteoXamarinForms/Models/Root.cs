using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace MeteoXamarinForms.Models
{
    public class Weather
    {
        [PrimaryKey, AutoIncrement]
        public int WeatherId { get; set; }

        [Column("main")]
        public string Main { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("icon")]
        public string Icon { get; set; }

        [ForeignKey(typeof(Current))]
        public int CurrentId { get; set; }

        [ForeignKey(typeof(Hourly))]
        public int HourlyId { get; set; }

        [ForeignKey(typeof(Daily))]
        public int DailyId { get; set; }

    }
    
    public class Current
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("unixDateTime")]
        public int Dt { get; set; }

        [Column("sunrise")]
        public int Sunrise { get; set; }

        [Column("sunset")]
        public int Sunset { get; set; }

        [Column("temperature")]
        public double Temp { get; set; }

        [Column("feelsLike")]
        public double Feels_Like { get; set; }

        [Column("pressure")]
        public int Pressure { get; set; }

        [Column("humidity")]
        public int Humidity { get; set; }

        [Column("dewPoint")]
        public double Dew_Point { get; set; }

        [Column("uvindex")]
        public double Uvi { get; set; }

        [Column("clouds")]
        public int Clouds { get; set; }

        [Column("visibility")]
        public int Visibility { get; set; }
        
        [Column("windSpeed")]
        public double Wind_Speed { get; set; }

        [Column("windDegre")]
        public int Wind_Deg { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Weather> Weather { get; set; }
    }

    public class Rain
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double _1h { get; set; }
    }

    public class Hourly
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("unixDateTime")]
        public int Dt { get; set; }

        [Column("temperature")]
        public double Temp { get; set; }

        [Column("feelsLike")]
        public double Feels_Like { get; set; }

        [Column("pressure")]
        public int Pressure { get; set; }

        [Column("humidity")]
        public int Humidity { get; set; }

        [Column("dewPoint")]
        public double DewPoint { get; set; }

        [Column("uvindex")]
        public double Uvi { get; set; }

        [Column("clouds")]
        public int Clouds { get; set; }

        [Column("visibility")]
        public int Visibility { get; set; }

        [Column("windSpeed")]
        public double Wind_Speed { get; set; }

        [Column("windDegre")]
        public int Wind_Deg { get; set; }

        [Column("windGust")]
        public double Wind_Gust { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Weather> Weather { get; set; }

        [Column("pop")]
        public double Pop { get; set; }

        [ForeignKey(typeof(Rain))]
        public int RainId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Rain Rain { get; set; }

        [ForeignKey(typeof(Root))]
        public int RootId { get; set; }
    }

    public class Temp
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("day")]
        public double Day { get; set; }

        [Column("min")]
        public double Min { get; set; }

        [Column("max")]
        public double Max { get; set; }

        [Column("night")]
        public double Night { get; set; }

        [Column("eve")]
        public double Eve { get; set; }

        [Column("morn")]
        public double Morn { get; set; }
    }
    
    public class FeelsLike
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("day")]
        public double Day { get; set; }

        [Column("night")]
        public double Night { get; set; }

        [Column("eve")]
        public double Eve { get; set; }

        [Column("morn")]
        public double Morn { get; set; }
    }

    public class Daily
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        [Column("unixDateTime")]
        public int Dt { get; set; }

        [Column("sunrise")]
        public int Sunrise { get; set; }

        [Column("sunset")]
        public int Sunset { get; set; }

        [Column("moonrise")]
        public int Moonrise { get; set; }

        [Column("moonset")]
        public int Moonset { get; set; }

        [Column("moonPhase")]
        public double Moon_Phase { get; set; }

        [ForeignKey(typeof(Temp))]
        public int TempId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Temp Temp { get; set; }

        [ForeignKey(typeof(FeelsLike))]
        public int Feels_LikeId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public FeelsLike Feels_Like { get; set; }

        [Column("pressure")]
        public int Pressure { get; set; }

        [Column("humidity")]
        public int Humidity { get; set; }

        [Column("dewPoint")]
        public double Dew_Point { get; set; }

        [Column("windSpeed")]
        public double Wind_Speed { get; set; }

        [Column("windDegre")]
        public int Wind_Deg { get; set; }

        [Column("windGust")]
        public double Wind_Gust { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Weather> Weather { get; set; }

        [Column("clouds")]
        public int Clouds { get; set; }

        [Column("pop")]
        public double Pop { get; set; }

        [Column("rain")]
        public double Rain { get; set; }

        [Column("uvindex")]
        public double Uvi { get; set; }

        [ForeignKey(typeof(Root))]
        public int RootId { get; set; }
    }

    public class Root
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("latitue")]
        public double Lat { get; set; }
        
        [Column("longitude")]
        public double Lon { get; set; }

        [Column("timezone")]
        public string Timezone { get; set; }

        [Column("timezoneOffset")]
        public int Timezone_Offset { get; set; }

        [ForeignKey(typeof(Current))]
        public int CurrentId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Current Current { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Hourly> Hourly { get; set; }
        
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Daily> Daily { get; set; }
    }
}
