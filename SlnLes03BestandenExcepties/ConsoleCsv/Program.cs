using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCsv
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            string[] Games = { "Schaken", "Backgammon", "Dammen" };
            string[,] teams = new string[,] { { "Peter", "Jan", "Lukas", "Marie", "Kobe" }, { "Axel", "Pieter-Jan", "Serhat", "Niels", "Amber" } };
            string Noteer = "";
            for (int i = 0; i < 100; i++)
            {
                int game = rnd.Next(1, Games.Length);
                int team = rnd.Next(0, 2);
                int player = rnd.Next(0, 5);
                int scndTeam;
                int scndPlayer = rnd.Next(0, 5);
                do
                {
                    scndTeam = rnd.Next(0, 2);
                }
                while (team != scndTeam);
                Noteer += Convert.ToString(i + 1) + "; " + Convert.ToString(teams[team, player]) + ";" + Convert.ToString(teams[scndTeam, scndPlayer]) + ";" + Convert.ToString(Games[game]) + ";" + Convert.ToString(rnd.Next(0, 5)) + "-" + Convert.ToString(rnd.Next(0, 5)) + Environment.NewLine;
            }
            Console.WriteLine(Noteer);
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = System.IO.Path.Combine(folderPath, "wedstrijden.csv");
            File.WriteAllText(filePath, Noteer);
            Console.ReadLine();
        }
    }
}
