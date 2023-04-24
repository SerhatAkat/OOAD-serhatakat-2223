using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKassaTicket
{
    public enum Betaalwijze { visa, cash, bancontact }

    internal class Ticket
    {
        public List<Product> Producten { get; } = new List<Product>();
        public Betaalwijze BetaaldMet { get; }
        public string Kassier { get; }
        public decimal Totaalprijs
        {
            get
            {
                decimal totaal = 0;
                foreach (Product product in Producten)
                {
                    totaal += product.Eenheidsprijs;
                }
                if (BetaaldMet == Betaalwijze.visa)
                {
                    totaal += 0.12m;
                }
                return totaal;
            }
        }

        public Ticket(string kassier, Betaalwijze betaalwijze)
        {
            Kassier = kassier;
            BetaaldMet = betaalwijze;
        }

        public void VoegProductToe(Product product)
        {
            Producten.Add(product);
        }

        public void DrukTicket()
        {
            Console.WriteLine("KASSATICKET");
            Console.WriteLine("===========");
            Console.WriteLine($"Uw Kassier: {Kassier}\n");

            foreach (Product product in Producten)
            {
                Console.WriteLine(product);
            }

            Console.WriteLine("--------------");

            if (BetaaldMet == Betaalwijze.visa)
            {
                Console.WriteLine("Visa kosten: 0,12");
            }

            Console.WriteLine($"Totaal: {Totaalprijs:0.00}");
        }
    }
}
