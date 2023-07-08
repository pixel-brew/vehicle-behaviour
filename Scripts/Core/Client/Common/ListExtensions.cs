using System;
using System.Collections.Generic;

namespace Core.Client
{
    public static class ListExtensions
    {
        public static bool Contains<T>(this List<T> list, Predicate<T> condition)
        {
            for (var index = 0; index < list.Count; index++)
            {
                if(condition(list[index]))
                {
                    return true;
                }
            }
            return false;
        }
        
        public static bool RemoveFirst<T>(this List<T> list, Predicate<T> condition)
        {
            for (var index = 0; index < list.Count; index++)
            {
                if(condition(list[index]))
                {
                    list.RemoveAt(index);
                    return true;
                }
            }

            return false;
        }
        
        public static int RemoveAll<T>(this List<T> list, Predicate<T> condition)
        {
            int deletedItemsCount = 0;
            for (var index = list.Count - 1; index >= 0; )
            {
                if(condition(list[index]))
                {
                    list.RemoveAt(index);
                    ++deletedItemsCount;
                }
                else
                {
                    index--;
                }
            }

            return deletedItemsCount;
        }
    }
}