using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        internal protected void InsertNewItem(int index, T newItem)
        {
            this.InsertItem(index, newItem);
        }
        internal protected void SetNewItem(int index, T newItem)
        {
            this.SetItem(index, newItem);
        }
        public K Key { get; private set; }
    }
}
