using System;
using System.Collections.Generic;

namespace ConsoleVeiling
{
    internal class Item
    {
        private bool eersteBod = false;

        public Koper Winnaar { get; set; }
        public Bod WinnendBod { get; set; }
        public Bod HoogsteBod { get; set; } = new Bod(null, 0);

        public decimal minBod { get; set; }
        public string Naam { get; set; }
        public Bod Bod { get; set; }


        public Item(string naam, int minimumBod)
        {
            Naam = naam;
            minBod = minimumBod;
        }

        public void PlaatsBod(Bod bod)
        {
            if (eersteBod)
            {
                eersteBod = false;
            }
            Bod = bod;
            if (Bod.Bodprijs > HoogsteBod.Bodprijs)
            {
                HoogsteBod = bod;
            }
        }



        public void SluitBiedingen()
        {
            WinnendBod = HoogsteBod;
            Winnaar = WinnendBod.Koper;
            HoogsteBod.Koper.AangeschafteItems.Add(this);
        }

    }
}
