using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKassaTicket
{
    internal class Product
    {
        public string Naam { get; }
        public decimal Eenheidsprijs { get; }
        public string Code { get; }

        public Product(string naam, decimal eenheidsprijs, string code)
        {
            if (ValideerCode(code))
            {
                Naam = naam;
                Eenheidsprijs = eenheidsprijs;
                Code = code;
            }
            else
            {
                throw new ArgumentException("Ongeldige productcode.");
            }
        }

        public static bool ValideerCode(string code)
        {
            return code.Length == 6 && code.StartsWith("P");
        }

        public override string ToString()
        {
            return $"({Code}) {Naam}: {Eenheidsprijs:0.00}";
        }
    }
}
