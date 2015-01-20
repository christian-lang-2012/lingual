using System;

namespace Lingual.Infrastructure
{
    public static class SafelyExtensions
    {
        public static T Recover<T>(this T item, Func<T> recoverFunc)
        {
            if (item == null)
            {
                return recoverFunc();
            }
            return item;
        }
        public static T If<T>(this T item, Func<T,bool> cond)
            where T : class
        {
            if (item.Let(cond, false))
            {
                return item;
            }
            return null;
        }
        public static T IfDo<T>(this T item, Func<T,bool> cond, Action<T> action)
        {
            if (item.Let(cond, false))
            {
                action(item);
            }
            return item;
        }
        public static K Let<T,K>(this T item, Func<T,K> transform, K failValue)
        {
            if (item == null)
            {
                return failValue;
            }
            return transform(item);
        }
        public static K Let<T,K>(this T item, Func<T,K> transform)
            where K : class
        {
            if (item == null)
            {
                return null;
            }
            return transform(item);
        }
        public static K TryLet<T,K>(this T item, Func<T,K> transform, Action<Exception> handler = null)
            where K : class
        {
            try
            {
                return transform(item);
            }
            catch(Exception e)
            {
                if (handler != null)
                {
                    handler(e);
                }
                return null;
            }
        }
    }
}

