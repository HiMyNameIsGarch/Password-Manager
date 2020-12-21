using System.Collections.Generic;

namespace PassManager.Models
{
    internal static class ManageFlyoutItems
    {
        private static Dictionary<string, bool> Items = new Dictionary<string, bool>();
        internal static void Add(string key, bool value)
        {
            if (!Items.ContainsKey(key))
            {
                Items.Add(key, value);
            }
        }
        internal static bool GetValue(string key)
        {
            if (Items.ContainsKey(key))
            {
                return Items[key];
            }
            return false;
        }
    }
}
