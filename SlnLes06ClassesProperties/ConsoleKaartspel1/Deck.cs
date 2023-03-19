using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKaartspel1
{
    internal class Deck
    {
        private List<Kaart> kaarten;

        public List<Kaart> Kaarten
        {
            get { return kaarten; }
        }

        public Deck()
        {
            kaarten = new List<Kaart>();
            for (int nummer = 1; nummer <= 13; nummer++)
            {
                kaarten.Add(new Kaart(nummer, "C"));
                kaarten.Add(new Kaart(nummer, "S"));
                kaarten.Add(new Kaart(nummer, "H"));
                kaarten.Add(new Kaart(nummer, "D"));
            }
        }

        public void Schudden()
        {
            Random random = new Random();
            int n = kaarten.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Kaart kaart = kaarten[k];
                kaarten[k] = kaarten[n];
                kaarten[n] = kaart;
            }
        }

        public Kaart NeemKaart()
        {
            if (kaarten.Count == 0)
            {
                throw new InvalidOperationException("Geen kaarten meer in het deck.");
            }
            Kaart kaart = kaarten[0];
            kaarten.RemoveAt(0);
            return kaart;
        }
    }
}
