using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RefrigeratorExercise
{
    internal class Refrigerator
    {
        private static List<Refrigerator> AllRefrigerators = new List<Refrigerator>(); // Static list to store all refrigerators
        private static int NextRefrigeratorId = 1; // A static field to track the next available ID.
        private int RefrigeratorId { get;}
        public string Model { get; }
        public string Color { get; }
        public int NumberOfShelves { get; set; }
        public List<Shelf> Shelves { get; }
        public Refrigerator(string model, string color)
        {
            RefrigeratorId = NextRefrigeratorId;
            NextRefrigeratorId++;
            Model = model;
            Color = color;
            Shelves = new List<Shelf>();// Initializing the Shelves property as an empty list.
            AllRefrigerators.Add(this);// Add the created refrigerator to the list of all refrigerators

        }
        public void AddShelf(Shelf shelf)
        {
            Shelves.Add(shelf);
            NumberOfShelves++;
        }
       

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Refrigerator ID: {RefrigeratorId}");
            sb.AppendLine($"Model: {Model}");
            sb.AppendLine($"Color: {Color}");
            sb.AppendLine($"Number of Shelves: {NumberOfShelves}");
            sb.AppendLine("Shelves:");
             foreach (var shelf in Shelves)
             {
                 sb.AppendLine(shelf.ToString());
             }
             
            return sb.ToString();
        }
         public double GetRemainingSpace()
         {
             double UsedSpace= Shelves.Sum(Shelf => Shelf.CurrentSpaceInShelfInSq);
             return UsedSpace;
         }

       
        public void PutItemInRefrigerator(Item item)
        {
            // Find the first available shelf with enough space for the item
            Shelf availableShelf = Shelves.FirstOrDefault(shelf => shelf.CurrentSpaceInShelfInSq >= item.SpaceInSq);

            if (availableShelf != null)
            {
                availableShelf.Items.Add(item);
                item.ShelfOn = availableShelf;
                // Update the free space on the shelf
                availableShelf.CurrentSpaceInShelfInSq -= item.SpaceInSq;
                Console.WriteLine("The item added to the refrigerator and placed on Shelf ID: " + availableShelf.ShelfId);
            }
            else
            {
                Console.WriteLine("No available shelf with enough place to add the item.");
            }
        }

        public void RemoveItemFromRefrigerator(int itemID)
        {
            foreach (var shelf in Shelves)
            {
                Item ItemToRemove = shelf.Items.FirstOrDefault(Item => Item.ItemId == itemID);
                if (ItemToRemove != null)
                {
                    shelf.Items.Remove(ItemToRemove);// Remove the item from the shelf
                    ItemToRemove.ShelfOn = null;
                    shelf.CurrentSpaceInShelfInSq += ItemToRemove.SpaceInSq;//A space is freed up in the meter
                    Console.WriteLine("Dear user, here is your item back: " + ItemToRemove);
                    return; 
                }
            }
        }
        public void ThrowOutExpired()
        {
            DateTime currentDate = DateTime.Today;

            foreach (var shelf in Shelves)
            {
                Console.WriteLine("Checking items on Shelf ID: " + shelf.ShelfId);

                List<Item> itemsToRemove = new List<Item>();

                foreach (var item in shelf.Items)
                {
                    Console.WriteLine("Checking item: " + item.ItemName + " (Item ID: " + item.ItemId + ")");
                    if (item.ExpiryDate < currentDate)
                    {
                        Console.WriteLine("Dear user, here is an expired item I took out of the fridge: " + item.ItemName + " (Item ID: " + item.ItemId + ")");
                        itemsToRemove.Add(item);
                        shelf.CurrentSpaceInShelfInSq += item.SpaceInSq;
                    }
                }
                foreach (var itemToRemove in itemsToRemove)// Remove the expired items from the shelf
                {
                    shelf.Items.Remove(itemToRemove);
                    itemToRemove.ShelfOn = null;

                }
            }
        }


        public List<Item> WhatIWantToEat(KosherType kosher, ItemType type)
        {
            ThrowOutExpired();//Make sure we don't return expired foods
            List<Item> matchingFoods = new List<Item>();
            foreach (var shelf in Shelves)
            {
                foreach (var item in shelf.Items)
                {
                    if (item.itemType == type && item.CosherType == kosher)
                    {
                        matchingFoods.Add(item);
                    }
                }
            }
            return matchingFoods;
        }
        public List<Item> SortItemsByExpirationDate()
        {
            // Collect all items from all shelves into a single list
            List<Item> allItems = Shelves.SelectMany(shelf => shelf.Items).ToList();

            // Sort the items by expiration date in ascending order
            List<Item> sortedItems = allItems.OrderBy(item => item.ExpiryDate).ToList();

            return sortedItems;
        }
        public List<Shelf> SortShelvesByFreeSpace()
        {
            // Sort the shelves by free space from largest to smallest
            List<Shelf> sortedShelves = Shelves.OrderByDescending(shelf => shelf.CurrentSpaceInShelfInSq).ToList();
            return sortedShelves;
        }
        public static List<Refrigerator> SortRefrigeratorsByFreeSpace()
        {
            List<Refrigerator> sortedRefrigerators = AllRefrigerators
                .OrderByDescending(refrigerator => refrigerator.Shelves.Sum(shelf => shelf.CurrentSpaceInShelfInSq))
                .ToList();

            return sortedRefrigerators;
        }
        public void GettingReadyForShopping()
        {

            if (GetRemainingSpace() >= 20)
            {
                Console.WriteLine("There's enough space in the refrigerator for shopping.");
                return;
            }

            ThrowOutExpired();

            if (GetRemainingSpace() >= 20)
            {
                Console.WriteLine("After throwing the expired date items, there's enough space in the refrigerator for shopping.");
                return;
            }

            // Check if there will be room after removing specific items
            bool isPlace = WillTherePlaceIfThrow();

            if (isPlace)
            {
                ThrowThreeDaysExpiryDairyItems();
                if (GetRemainingSpace() >= 20)
                {
                    Console.WriteLine("Ready to shopping!");
                }
            }
            else
            {
                Console.WriteLine("There is no room at all in the fridge, put off shopping.");
            }
        }


        public bool WillTherePlaceIfThrow()
        {
            double NowFreePlace = GetRemainingSpace();
            double ItemsSpace = 0;
            ItemsSpace += CalculateItemsSpaceByCategory(KosherType.Dairy, 3);
            ItemsSpace += CalculateItemsSpaceByCategory(KosherType.Meat, 7);
            ItemsSpace += CalculateItemsSpaceByCategory(KosherType.Parve, 1);
            if ((NowFreePlace + ItemsSpace) < 20)
                return false;
            else
                return true;
        }
        public double CalculateItemsSpaceByCategory(KosherType category, int DaysUntilExpires)
        {
            DateTime currentDate = DateTime.Today;
            double totalSpace = 0;

            foreach (var shelf in Shelves)
            {
                totalSpace += shelf.Items
                    .Where(item => item.CosherType == category && (item.ExpiryDate - currentDate).TotalDays < DaysUntilExpires)
                    .Sum(item => item.SpaceInSq);
            }

            return totalSpace;
        }


        public void ThrowThreeDaysExpiryDairyItems()
        {
            DateTime currentDate = DateTime.Today;

            foreach (var shelf in Shelves)
            {
                foreach (var item in shelf.Items.ToList()) // Use ToList() to create a copy of the list for safe removal
                {
                    if (item.CosherType == KosherType.Dairy && (item.ExpiryDate - currentDate).Days < 3)
                    {
                        shelf.CurrentSpaceInShelfInSq += item.SpaceInSq;
                        shelf.Items.Remove(item);
                        Console.WriteLine("Removed a dairy item that expires within 3 days.");
                    }
                }
            }

            if (GetRemainingSpace() < 20)
            {
            
                ThrowWeekExpiryMeatItems();
            }
            else
            {
                Console.WriteLine("Ready to shop!");
            }
        }


        public void ThrowWeekExpiryMeatItems()
        {
            DateTime currentDate = DateTime.Today;

            foreach (var shelf in Shelves)
            {
                foreach (var item in shelf.Items.ToList()) 
                {
                    if (item.CosherType == KosherType.Meat && (item.ExpiryDate - currentDate).Days < 7)
                    {
                        shelf.CurrentSpaceInShelfInSq += item.SpaceInSq;
                        shelf.Items.Remove(item);
                        Console.WriteLine("Removed a meat item that expires within a week.");
                    }
                }
            }

            if (GetRemainingSpace() < 20)
            {
              
                ThrowDayExpiryParveItems();
            }
            else
            {
                Console.WriteLine("Ready to shopping!");
            }
        }


        public void ThrowDayExpiryParveItems()
        {
            DateTime currentDate = DateTime.Today;

            foreach (var shelf in Shelves)
            {
                foreach (var item in shelf.Items.ToList()) 
                {
                    if (item.CosherType == KosherType.Parve && (item.ExpiryDate - currentDate).Days < 1)
                    {
                        shelf.CurrentSpaceInShelfInSq += item.SpaceInSq;
                        shelf.Items.Remove(item);
                        Console.WriteLine("Removed a parve item that expires within a day.");
                    }
                }
            }

            if (GetRemainingSpace() < 20)
            {
                Console.WriteLine("There is no room at all in the fridge - this is not the time to shop!");
            }
            else
            {
                Console.WriteLine("Ready to shopping!");
            }
        }



        public int FindItemIdByItemName(string productName)
        {
            foreach (var shelf in Shelves)
            {
                var item = shelf.Items.FirstOrDefault(i => i.ItemName == productName);
                if (item != null)
                {
                    return item.ItemId; 
                }
            }

            throw new Exception("Product not found"); 
        }

    }
}
