using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKassaTicket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Product bananen = new Product("bananen", 1.75m, "P02384");
            Product brood = new Product("brood", 2.10m, "P01820");
            Product kaas = new Product("kaas", 3.99m, "P45612");
            Product koffie = new Product("koffie", 4.10m, "P98754");

            string[] kassiers = { "Annie", "Bob", "Charlie" };
            Random random = new Random();
            int randomIndex = random.Next(kassiers.Length);
            string gekozenKassier = kassiers[randomIndex];

            Array betaalwijzen = Enum.GetValues(typeof(Betaalwijze));
            Betaalwijze willekeurigeBetaalwijze = (Betaalwijze)betaalwijzen.GetValue(random.Next(betaalwijzen.Length));

            Ticket ticket = new Ticket(gekozenKassier, willekeurigeBetaalwijze);
            ticket.VoegProductToe(bananen);
            ticket.VoegProductToe(brood);
            ticket.VoegProductToe(kaas);
            ticket.VoegProductToe(koffie);

            ticket.DrukTicket();
            Console.ReadLine();
        }
    }
}
