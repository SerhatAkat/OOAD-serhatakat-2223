using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
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
            string[] players = { "Zakaria", "Saleha", "Indra", "Ralph", "Francisco", "Marie", "Jens", "Serhat", "Niels", "Amber" };
            string Noteer = "";
            for (int i = 0; i < 100; i++)
            {
                string player1, player2;
                int game = rnd.Next(0, Games.Length);
                int playerIndex1, playerIndex2;
                do
                {
                    playerIndex1 = rnd.Next(0, players.Length);
                    player1 = players[playerIndex1];
                    playerIndex2 = rnd.Next(0, players.Length);
                    player2 = players[playerIndex2];
                }
                while (player1 == player2); // spelers moeten verschillende namen hebben
                int score1 = rnd.Next(0, 3);
                int score2 = 3 - score1;
                Noteer += Convert.ToString(i + 1) + "; " + player1 + ";" + player2 + ";" + Convert.ToString(Games[game]) + ";" + Convert.ToString(score1) + "-" + Convert.ToString(score2) + Environment.NewLine;
            }
            Console.WriteLine(Noteer);
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = System.IO.Path.Combine(folderPath, "wedstrijden.csv");
            File.WriteAllText(filePath, Noteer);
            Console.ReadLine();
        }
    }
}