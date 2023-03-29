using System;
using System.Collections.Generic;

namespace ConsoleVeiling
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            Random rnd2 = new Random();
            Console.WriteLine(@"
VINTAGE MEUBELS VEILING
=======================
");


            Koper koper1 = new Koper("Serhat");
            Koper koper2 = new Koper("Jens");
            Koper koper3 = new Koper("Stefan");


            Item item1 = new Item("Antieke Vaas", 0);
            Item item2 = new Item("Gouden Ketting", 0);


            item1.PlaatsBod(new Bod(koper3, rnd.Next(0, 1000)));
            item1.PlaatsBod(new Bod(koper1, rnd.Next(Convert.ToInt32(item1.HoogsteBod.Bodprijs), 1200)));
            item1.PlaatsBod(new Bod(koper2, rnd.Next(Convert.ToInt32(item1.HoogsteBod.Bodprijs), 1400)));
            item1.SluitBiedingen();
            Bod winnendBod = item1.WinnendBod;
            if (winnendBod == null)
            {
                Console.WriteLine($"De {item1.Naam} is niet verkocht geraakt");
            }
            else
            {
                Console.WriteLine($"De {item1.Naam} is verkocht voor {winnendBod.Bodprijs} euro");
            }

            item2.PlaatsBod(new Bod(koper2, rnd2.Next(1000, 1500)));
            item2.PlaatsBod(new Bod(koper3, rnd2.Next(Convert.ToInt32(item2.HoogsteBod.Bodprijs), 1500)));
            item2.PlaatsBod(new Bod(koper1, rnd2.Next(Convert.ToInt32(item2.HoogsteBod.Bodprijs), 2000)));
            item2.SluitBiedingen();
            winnendBod = item2.WinnendBod;

            if (winnendBod == null)
            {
                Console.WriteLine($"De {item2.Naam} is niet verkocht geraakt");
            }
            else
            {
                Console.WriteLine($"De {item2.Naam} is verkocht voor {winnendBod.Bodprijs} euro");
            }
            Console.WriteLine();
            List<Koper> Kopers = new List<Koper> { koper1, koper2, koper3 };
            foreach (Koper koper in Kopers)
            {
                Console.Write($"Koper {koper.Naam} heeft de volgende items in bezit: ");
                foreach (Item item in koper.AangeschafteItems)
                {
                    if (item.WinnendBod.Koper == koper)
                    {
                        {
                            Console.WriteLine($"De {item.Naam}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Geen items");
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
