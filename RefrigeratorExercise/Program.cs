using RefrigeratorExercise;

class Program
{
    static void Main(string[] args)
    {
        Refrigerator fridge1 = new Refrigerator("Samsung", "White");
        Shelf shelf1 = new Shelf(20, 1);
        Shelf shelf2 = new Shelf(25, 2);
        fridge1.AddShelf(shelf1);
        fridge1.AddShelf(shelf2);

        Refrigerator fridge2 = new Refrigerator("Samsung", "White");
        Shelf shelf3 = new Shelf(5, 1);
        Shelf shelf4 = new Shelf(6, 2);
        fridge2.AddShelf(shelf3);
        fridge2.AddShelf(shelf4);

        while (true)
        {
            Console.WriteLine("Refrigerator Menu:");
            Console.WriteLine("1: Show all the contents of the refrigerator");
            Console.WriteLine("2: Show remaining space in the refrigerator");
            Console.WriteLine("3: Put an item in the fridge");
            Console.WriteLine("4: Remove an item from the refrigerator");
            Console.WriteLine("5: The refrigerator is being cleaned...");
            Console.WriteLine("6: What do I want to eat?");
            Console.WriteLine("7: Sort items by expiration date: ");
            Console.WriteLine("8: Sort shelves by free space: ");
            Console.WriteLine("9: Sort refrigerators by free space");
            Console.WriteLine("10: Prepare the refrigerator for shopping");
            Console.WriteLine("100: System shutdown");

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        Console.WriteLine(fridge1.ToString());
                        break;
                    case 2:
                        Console.WriteLine($"Remaining space in the refrigerator: {fridge1.GetRemainingSpace()} square meters");
                        break;
                    case 3:
                        PutItemInFridge(fridge1);
                        break;
                    case 4:
                        RemoveItemFromRefrigerator(fridge1);
                        break;
                    case 5:
                        CleanRefrigerator(fridge1);
                        break;
                    case 6:
                        WhatToEat(fridge1);
                        break;
                    case 7:
                        SortItemsByExpirationDate(fridge1);
                        break;
                    case 8:
                        SortShelvesByFreeSpace(fridge1);
                        break;
                    case 9:
                        SortRefrigeratorsByFreeSpace();
                        break;
                    case 10:
                        fridge1.GettingReadyForShopping();
                        break;
                    case 100:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please choose a valid option.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid menu option.");
            }
        }
    }
    static void SortItemsByExpirationDate(Refrigerator fridge)
    {
        List<Item> SortedItems = fridge.SortItemsByExpirationDate();
        Console.WriteLine("Items sorted by expiration date:");
        foreach (var item in SortedItems)
        {
            Console.WriteLine(item.ToString());
        }
    }
    static void SortShelvesByFreeSpace(Refrigerator fridge)
    {
        List<Shelf> SortedShelves = fridge.SortShelvesByFreeSpace();
        Console.WriteLine("Shelves sorted by free space:");
        foreach (var shelf in SortedShelves)
        {
            Console.WriteLine(shelf.ToString());
        }
    }
    static void SortRefrigeratorsByFreeSpace()
    {
        List<Refrigerator> SortedRefrigerators = Refrigerator.SortRefrigeratorsByFreeSpace();
        Console.WriteLine("Refrigerators sorted by free space:");
        foreach (var refrigerator in SortedRefrigerators)
        {
            Console.WriteLine(refrigerator.ToString());
        }
    }

    static void PutItemInFridge(Refrigerator fridge)
    {
        Console.WriteLine("Enter the name of the item you want to put:");
        string itemName = Console.ReadLine();

        Console.WriteLine("Enter the item type (Food or Drink):");
        string itemTypeInput = Console.ReadLine();
        if (Enum.TryParse(itemTypeInput, out ItemType itemType))
        {
            Console.WriteLine("Enter the Kosher type (Meat, Dairy, or Parve):");
            string kosherTypeInput = Console.ReadLine();
            if (Enum.TryParse(kosherTypeInput, out KosherType kosherType))
            {
                Console.WriteLine("Enter the expiry date (yyyy-MM-dd):");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime expiryDate))
                {
                    Console.WriteLine("Enter the space required (sq.m.):");
                    if (double.TryParse(Console.ReadLine(), out double spaceInSq))
                    {
                        fridge.PutItemInRefrigerator(new Item(itemName)
                        {
                            itemType = itemType,
                            CosherType = kosherType,
                            ExpiryDate = expiryDate,
                            SpaceInSq = spaceInSq
                        });
                    }
                    else
                    {
                        Console.WriteLine("Invalid space input. Please enter a valid number.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please enter a valid date (yyyy-MM-dd).");
                }
            }
            else
            {
                Console.WriteLine("Invalid Kosher type. Please enter a valid type.");
            }
        }
        else
        {
            Console.WriteLine("Invalid item type. Please enter a valid type.");
        }
    }

    static void RemoveItemFromRefrigerator(Refrigerator fridge)
    {
        Console.WriteLine("Enter a name of item you want to remove");
        string ItemNameRemove = Console.ReadLine();
        int ItemID = fridge.FindItemIdByItemName(ItemNameRemove);
        fridge.RemoveItemFromRefrigerator(ItemID);
    }

    static void CleanRefrigerator(Refrigerator fridge)
    {
        fridge.ThrowOutExpired();
        Console.WriteLine("Refrigerator cleaned!");
    }

    static void WhatToEat(Refrigerator fridge)
    {
        Console.WriteLine("Enter the type of item (Food or Drink):");
        string ItemTypeInput = Console.ReadLine();
        if (Enum.TryParse(ItemTypeInput, out ItemType ItemType))
        {
            Console.WriteLine("Enter the Kosher type (Meat, Dairy, or Parve):");
            string kosherTypeInput = Console.ReadLine();
            if (Enum.TryParse(kosherTypeInput, out KosherType kosherType))
            {
                List<Item> MatchingItems = fridge.WhatIWantToEat(kosherType, ItemType);

                if (MatchingItems.Count > 0)
                {
                    Console.WriteLine("You have the following items matching your criteria:");
                    foreach (var item in MatchingItems)
                    {
                        Console.WriteLine(item.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("No items match your criteria.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Kosher type. Please enter a valid type.");
            }
        }
        else
        {
            Console.WriteLine("Invalid item type. Please enter a valid type.");
        }
    }

}
