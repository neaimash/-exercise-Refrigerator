using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefrigeratorExercise
{
    internal class Shelf
    {
        private static int NextShelfId = 1; // A static field to track the next available ID.
        public int ShelfId { get; }
        public int FloorNumber { get; set; }
        public double SpaceInShelfInSq { get; set; }
        public double CurrentSpaceInShelfInSq { get; set; }
        public List<Item> Items { get; set;}
        public Shelf(double spaceInShelfInSq, int floorNumber)
        {
            ShelfId = NextShelfId;
            NextShelfId++;
            FloorNumber = floorNumber;
            SpaceInShelfInSq = spaceInShelfInSq;
            CurrentSpaceInShelfInSq = SpaceInShelfInSq ; // Set CurrentSpaceInShelfInSq equal to SpaceInShelfInSq
            Items = new List<Item>(); // Initialize the Items property in the constructor.
        }
     
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Shelf ID: {ShelfId}");
            sb.AppendLine($"Floor Number: {FloorNumber}");
            sb.AppendLine($"Available Space (sq.m.): {CurrentSpaceInShelfInSq}");
            sb.AppendLine("Items on the Shelf:");
            foreach (var item in Items)
            {
                sb.AppendLine(item.ToString());
            }
            return sb.ToString();
        }




    }
}
