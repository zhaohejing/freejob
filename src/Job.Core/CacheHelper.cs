using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace HuaTong.General.Utility {
    /// <summary>
    /// 缓存操作,默认缓存1分钟
    /// </summary>
    public static class CacheHelper {
        static int cacheTime = 1;

        /// <summary>
        /// 读取缓存项
        /// </summary>
        /// <returns></returns>
        public static object CacheReader(string cacheKey) {
            return HttpRuntime.Cache[cacheKey];
        }

        /// <summary>
        /// 写入缓存项
        /// </summary>
        public static void CacheWriter(string cacheKey, object cacheValue, int cache_time = 0) {
            HttpRuntime.Cache.Insert(cacheKey, cacheValue, null,
                DateTime.Now.AddMinutes(cache_time <= 0 ? cacheTime : cache_time),
                Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 移除指定缓存项
        /// </summary>
        public static void CacheRemove(string cacheName) {
            HttpRuntime.Cache.Remove(cacheName);
        }

        /// <summary>
        /// 缓存对象泛型实现
        /// </summary>
        public static T ObjectReader<T>(string cacheKey = "")
            where T : class {
            string cachekey = typeof(T).GetHashCode() + cacheKey;
            var obj = CacheReader(cachekey) as T;
            return obj;
        }

        /// <summary>
        /// 缓存对象泛型实现
        /// </summary>
        public static void ObjectWriter<T>(T cacheValue, string cacheKey ="", int cache_time = 0)
            where T : class {
            string cachekey = typeof(T).GetHashCode() + cacheKey;
            CacheWriter(cachekey, cacheValue, cache_time);
        }
    }
}