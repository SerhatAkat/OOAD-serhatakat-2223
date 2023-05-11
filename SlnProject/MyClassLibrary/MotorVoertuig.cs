using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class MotorVoertuig
    {
        public int Id { get; }
        public int Voertuig_Id { get; }

        public enum Transmissie
        {
            Manueel,
            Automatisch
        }

        public enum Brandstof
        {
            Benzine,
            Diesel
        }
    }
}
