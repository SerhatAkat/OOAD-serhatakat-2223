using System;
using System.Collections.Generic;

namespace ConsoleVeiling
{
    internal class Bod
    {
        public Bod(Koper koper, Decimal bodwaarde)
        {
            Koper = koper;
            Bodprijs = bodwaarde;
        }
        public Koper Koper { get; set; }
        public decimal Bodprijs { get; set; }
    }
}
