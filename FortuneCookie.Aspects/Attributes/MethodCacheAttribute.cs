using System;
using FortuneCookie.Aspects.Caching;

namespace FortuneCookie.Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MethodCacheAttribute : Attribute
    {
        public static int DefaultDurationInMinutes = 60;
        
        private int? _durationInSeconds;
        private CacheLocation _cacheLocation = CacheLocation.Cache;

        public int DurationInMinutes
        {            
            get
            {
                return (_durationInSeconds.HasValue && _durationInSeconds.Value > 0) ? _durationInSeconds.Value / 60 : DefaultDurationInMinutes;
            }
            set { _durationInSeconds = value * 60; }
        }

        public int DurationInSeconds
        {
            get
            {
                return _durationInSeconds.HasValue ? _durationInSeconds.Value : DefaultDurationInMinutes * 60;
            }
            set { _durationInSeconds = value; }
        }

        public CacheLocation CacheLocation
        {
            get { return _cacheLocation; }
            set { _cacheLocation = value; }

        }
    }
}