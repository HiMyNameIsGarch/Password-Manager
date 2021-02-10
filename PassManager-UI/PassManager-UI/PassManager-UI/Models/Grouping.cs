using PassManager.Models.Items;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PassManager.Models
{
    public class Grouping<K, T> : ObservableCollection<T>
    {
        public Grouping() { }
        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
            {
                this.Items.Add(item);
            }
        }
        public static IEnumerable<Grouping<string, ItemPreview>> AddKeys(IEnumerable<Grouping<string, ItemPreview>> items)
        {
            if (items is null || items.Count() == 0) return null;
            foreach (var item in items)
            {
                var firstItem = item.FirstOrDefault();
                if(firstItem != null)
                {
                    item.Key = firstItem.ItemType.ToPluralString();
                }
            }
            return items;
        }
        internal protected void SetNewItem(int index, T newItem)
        {
            this.SetItem(index, newItem);
        }
        public K Key { get; private set; }
    }
}
