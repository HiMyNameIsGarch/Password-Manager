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
        public static IEnumerable<Grouping<string, ItemPreview>> GroupList(IEnumerable<ItemPreview> items)
        {
            if (items is null || items.Count() == 0) return null;
            return items
                    .GroupBy(item => item.ItemType)
                    .Select(item => new Grouping<string, ItemPreview>(item.Key.ToSampleString(), item));
        }
        internal protected void SetNewItem(int index, T newItem)
        {
            this.SetItem(index, newItem);
        }
        public K Key { get; private set; }
    }
}
