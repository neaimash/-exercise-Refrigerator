using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefrigeratorExercise
{
    internal class Item
    {
        private static int NextItemId = 1; // A static field to track the next available ID.
        public int ItemId { get; }
        public string ItemName { get; } 
        public Shelf ShelfOn { get; set; } 
        public ItemType itemType { get; set; }
        public KosherType CosherType { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double SpaceInSq { get; set; }
        public Item( string itemName )
        {
            ItemName = itemName;
            ItemId = NextItemId;
            NextItemId++;
        }
        public override string ToString()
        {
            return $"Item ID: {ItemId}\nProduct Name: {ItemName}\nShelf: {ShelfOn?.ShelfId.ToString() ?? "N/A"}\nType: {itemType}\nKosher Type: {CosherType}\nExpiry Date: {ExpiryDate:yyyy-MM-dd}\nSpace Required (sq.m.): {SpaceInSq}\n";
        }
       
    }

    public enum ItemType
    {
        Food,
        Drink
    }

    public enum KosherType
    {
        Meat,
        Dairy,
        Parve
    }
  

}
