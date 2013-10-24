using System;
using System.Web;
using FortuneCookie.Aspects.Attributes;

namespace FortuneCookie.Aspects.Caching
{
    internal static class CacheHelper
    {
        public static object GetFromCache(string key, MethodCacheAttribute attribute)
        {
            if (HttpContext.Current == null)
                return null;

            object value = null;

            switch (attribute.CacheLocation)
            {
                case CacheLocation.Cache:
                    value = HttpContext.Current.Cache[key];
                    break;
                case CacheLocation.ContextItems:
                    value = HttpContext.Current.Items[key];
                    break;
                case CacheLocation.Session:
                    value = HttpContext.Current.Session[key];
                    break;
            }

            return value;
        }

        public static void AddToCache(string key, object value, MethodCacheAttribute attribute)
        {
            if (HttpContext.Current == null)
                return;

            switch (attribute.CacheLocation)
            {
                case CacheLocation.Cache:
                    HttpContext.Current.Cache.Insert(key, value, null, DateTime.Now.AddSeconds(attribute.DurationInSeconds), System.Web.Caching.Cache.NoSlidingExpiration);
                    break;
                case CacheLocation.ContextItems:
                    HttpContext.Current.Items[key] = value;
                    break;
                case CacheLocation.Session:
                    HttpContext.Current.Session[key] = value;
                    break;
            }
        }
    }
}