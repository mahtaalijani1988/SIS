﻿using System;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace Student_Information_System.Extentions.Caching
{
    public static class CacheManager
    {
        public static void CacheInsert(this HttpContextBase httpContext, string key, object data, int durationMinutes)
        {
            if (data == null) return;
            httpContext.Cache.Add(
                key,
                data,
                null,
                DateTime.Now.AddMinutes(durationMinutes),
                TimeSpan.Zero,
                CacheItemPriority.AboveNormal,
                null);
        }

        public static T CacheRead<T>(this HttpContextBase httpContext, string key)
        {
            var data = httpContext.Cache[key];
            if (data != null)
                return (T)data;
            return default(T);
        }

        public static void InvalidateCache(this HttpContextBase httpContext, string key)
        {
            httpContext.Cache.Remove(key);
        }
        public static void InvalidateOutPutCache(string url)
        {
            HttpResponse.RemoveOutputCacheItem(url);
        }

        public static void InvalidateChildActionsCache()
        {
            OutputCacheAttribute.ChildActionCache = new System.Runtime.Caching.MemoryCache(Guid.NewGuid().ToString());
        }
    }
}