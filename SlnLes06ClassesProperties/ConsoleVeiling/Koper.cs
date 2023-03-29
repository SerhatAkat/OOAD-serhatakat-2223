using System;
using System.Collections.Generic;

namespace ConsoleVeiling
{
    internal class Koper
    {
        private List<Item> aangeschafteItems = new List<Item>();
        public Koper(string naam)
        {
            Naam = naam;
            AangeschafteItems = aangeschafteItems;
        }
        public string Naam { get; set; }
        public List<Item> AangeschafteItems { get; set; }
    }
}
