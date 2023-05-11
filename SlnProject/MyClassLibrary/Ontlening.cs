using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class Ontlening
    {
        public int Id { get; }
        public DateTime Vanaf { get; set; }
        public DateTime Tot { get; set; }
        public string Bericht { get; set; }
        public int Voertuig_Id { get; }
        public int Aanvrager_Id { get; }

        public enum Status
        {
            InAanvraag,
            Goedgekeurd,
            Verworpen
        }
    }
}
