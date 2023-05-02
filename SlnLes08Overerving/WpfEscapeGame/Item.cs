using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    internal class Item : Actor
    {
        public bool IsPortable { get; set; } = true;
        public Item HiddenItem { get; set; }

        public Item(string name, string desc) : base(name, desc)
        {
        }

        public Item(string name) : base(name, string.Empty)
        {
        }

        public Item(string name, string desc, bool isPortable) : base(name, desc)
        {
            IsPortable = isPortable;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
