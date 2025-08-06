using System.Collections.Generic;

namespace LFramework
{
    public static class ListTool
    {
        /// <summary>
        /// 打乱集合
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> RandomSort<T>(this List<T> ListT)
        {
            System.Random random = new System.Random();
            List<T> newList = new List<T>();
            foreach (T item in ListT)
            {
                newList.Insert(random.Next(newList.Count + 1), item);
            }

            return newList;
        }

        public static T TryGetFirst<T>(this List<T> ListT)
        {
            return ListT.Count > 0 ? ListT[0] : default(T);
        }
    }
}