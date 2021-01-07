﻿using System.Collections.Generic;
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
        public K Key { get; private set; }
    }
}