namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    public static class Utils
    {
        public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
        {
            foreach (T entry in source)
            {
                target.Add(entry);
            }
        }
    }
}
