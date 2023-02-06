using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTafels
{
    internal class Program
    {
        private static void DrukTafel(int get1, int get2)
        {
            Console.WriteLine($"{get1}x{get2} tafel:");
            for (int i = 1; i <= get2; i++)
            {
                int maal = get1 * i;
                Console.WriteLine($"{get1} x {i} = {maal}");
            }
            Console.ReadLine();
        }
        private static void VraagPositiefGetal()
        {
            Console.Write($"Geef een getal: ");
            int get1 = Convert.ToInt32(Console.ReadLine());
            while (get1 < 0)
            {
                Console.Write($"Het getal moet positief zijn! Geef een getal: ");
                get1 = Convert.ToInt32(Console.ReadLine());
            }
            Console.Write("Geef een lengte: ");
            int lengte = Convert.ToInt32(Console.ReadLine());
            DrukTafel(get1, lengte);
            Console.ReadLine();
        }
        static void Main(string[] args)
        {
            DrukTafel(4, 8);
            DrukTafel(2, 5);
            VraagPositiefGetal();
        }
    }
}
