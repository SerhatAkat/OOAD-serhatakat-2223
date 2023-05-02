using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    internal class Key : Item
    {
        public string KeyId { get; set; }
        public string KeySize { get; set; }

        public Key(string name, string desc, string keyId, string keySize) : base(name, desc)
        {
            KeyId = keyId;
            KeySize = keySize;
        }
    }
}
