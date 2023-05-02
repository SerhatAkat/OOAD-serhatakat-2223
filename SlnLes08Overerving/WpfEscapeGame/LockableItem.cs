using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    internal class LockableItem : Item
    {
        public bool IsLocked { get; set; }
        public Item Key { get; set; }

        public LockableItem(string name, string desc, bool isLocked, Item key) : base(name, desc)
        {
            IsLocked = isLocked;
            Key = key;
        }
    }
}
