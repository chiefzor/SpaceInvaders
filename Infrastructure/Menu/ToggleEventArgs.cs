using System;

namespace Infrastructure.Menu
{
    public class ToggleEventArgs<K> : EventArgs
    {
        public K ItemValue { get; set; }

        public ToggleEventArgs(K i_Item)
        {
            ItemValue = i_Item;
        }
    }
}
