﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    internal class Room
    {
        public string Name { get; } // read-only: kan maar één keer ingesteld worden
        public string Description { get; }
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Door> Doors { get; set; } = new List<Door>(); // Nieuwe lijst voor deuren
        public string ImagePath { get; set; }


        public Room(string name, string desc, string imagePath)
        {
            Name = name;
            Description = desc;
            ImagePath = imagePath;
            Items = new List<Item>();
            Doors = new List<Door>();
        }


        public List<Room> FindConnectedRooms()
        {
            List<Room> connectedRooms = new List<Room>();

            foreach (Door door in Doors)
            {
                if (door.LeadsTo != null && !connectedRooms.Contains(door.LeadsTo))
                {
                    connectedRooms.Add(door.LeadsTo);
                }
            }

            return connectedRooms;
        }


    }
}
