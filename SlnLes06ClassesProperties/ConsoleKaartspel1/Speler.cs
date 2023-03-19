using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKaartspel1
{
    internal class Speler
    {
        private string naam;
        private List<Kaart> kaarten;

        public string Naam
        {
            get { return naam; }
        }

        public List<Kaart> Kaarten
        {
            get { return kaarten; }
        }

        public bool HeeftNogKaarten
        {
            get { return kaarten.Count > 0; }
        }

        public Speler(string naam)
        {
            this.naam = naam;
            kaarten = new List<Kaart>();
        }

        public Speler(string naam, List<Kaart> kaarten)
        {
            this.naam = naam;
            this.kaarten = kaarten;
        }

        public Kaart LegKaart()
        {
            if (kaarten.Count == 0)
            {
                throw new InvalidOperationException("Geen kaarten meer in de hand van de speler.");
            }
            Random random = new Random();
            int index = random.Next(kaarten.Count);
            Kaart kaart = kaarten[index];
            kaarten.RemoveAt(index);
            return kaart;
        }
    }
}
