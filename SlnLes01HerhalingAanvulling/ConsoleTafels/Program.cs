using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTafels
{
    internal class Program
    {
        private static string DrukTafel(int get1, int get2)
        {
            string str = $"{get1}x{get2} tafel: \n";
            for (int i = 1; i <= get2; i++)
            {
                int maal = get1 * i;
                str += $"{get1} x {i} = {maal} \n";
            }
            return str;
        }
        private static void VraagPositiefGetal()
        {
            int get1 = Convert.ToInt32(Console.ReadLine());
            while (get1 < 0)
            {
                Console.Write($"Het getal moet positief zijn! Geef een getal: ");
                get1 = Convert.ToInt32(Console.ReadLine());
            }
            Console.Write("Geef een lengte: ");
            int lengte = Convert.ToInt32(Console.ReadLine());
            DrukTafel(get1, lengte);
            Console.WriteLine(DrukTafel(get1, lengte));
        }
        static void Main(string[] args)
        {
            Console.WriteLine(DrukTafel(4, 8));
            Console.WriteLine(DrukTafel(2, 5));
            Console.WriteLine();
            Console.Write($"Geef een getal: ");
            VraagPositiefGetal();
            Console.ReadLine();
        }
    }
}