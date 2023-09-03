using MiniGame.Enums;
using MiniGame.Untils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MiniGame
{
    internal class Seller
    {
        private readonly Random _random = new Random();
        private readonly Inventory _inventory;
        private readonly Player Player;

        private Dictionary<string, string>[] _clothesItems;
        private Dictionary<string, string>[] _healItems;

        public Seller(Player player, Inventory inv)
        {
            _inventory = inv;
            Player = player;

            _clothesItems = new Dictionary<string, string>[]
            {
                GetNewClothes("armor", BodyPart.Helmet),
                GetNewClothes("armor", BodyPart.Body),
                GetNewClothes("armor", BodyPart.Legs),
                GetNewClothes("armor", BodyPart.Shield),
                GetNewClothes("damage", BodyPart.Weapon)
            };

            _healItems = new Dictionary<string, string>[]
            {
                new Dictionary<string, string> {
                    { "name", NamesItem.GetName("Heal", 0) },
                    { "units", "20" },
                    { "type", "heal" },
                    { "price", GetPrice(1).ToString() },
                    { "unitsForSale", "1" },
                    { "availableItems", "3"}
                },
                new Dictionary<string, string> {
                    { "name", NamesItem.GetName("Heal", 1) },
                    { "units", "50" },
                    { "type", "heal" },
                    { "price", GetPrice(2).ToString() },
                    { "unitsForSale", "2" },
                    { "availableItems", "2"}
                },
                new Dictionary<string, string> {
                    { "name", NamesItem.GetName("Heal", 2) },
                    { "units", "100" },
                    { "type", "heal" },
                    { "price", GetPrice(3).ToString() },
                    { "unitsForSale", "3" },
                    { "availableItems", "1"}
                },
                new Dictionary<string, string> {
                    { "name", NamesItem.GetName("Heal", 3) },
                    { "units", "999" },
                    { "type", "heal" },
                    { "price", GetPrice(4).ToString() },
                    { "unitsForSale", "4" },
                    { "availableItems", "1"}
                }
            };

            Console.WriteLine("You found a seller\nThe seller showed their items for sale.");
        }

        public void Start()
        {
            Console.WriteLine("Selling items:");

            ShowOneCategory("Clothes", 'C', _clothesItems);
            ShowOneCategory("Heal items", 'H', _healItems);

            List<string> ArrIndex = new List<string>() { "E", "I", "S", "L" };

            AddToArrIndexes('C', _clothesItems.Length, ref ArrIndex);
            AddToArrIndexes('H', _healItems.Length, ref ArrIndex);

            string Res = AskQuestion.Main("Would you like to buy something? Write the index of the item.\nTo look at your inventory: I;\nTo sell something to the seller: L;\nTo look at your statistics: S;\nWrite E to exit.", ArrIndex.ToArray());

            if (Res == "E")
                return;

            switch (Res)
            {
                case "I":
                    _inventory.ShowInventory();
                    break;
                case "S":
                    Player.ShowStatistics();
                    break;
                case "L":
                    _inventory.ShowInventory(isSeller: true);
                    break;
                default:
                    int CurrentIndex = int.Parse(Res.Substring(1)) - 1;

                    switch (Res[0])
                    {
                        case 'C':
                            BuyItem(_clothesItems[CurrentIndex], CurrentIndex, isBodyPart: true);
                            break;
                        case 'H':
                            BuyItem(_healItems[CurrentIndex], CurrentIndex, isBodyPart: false);
                            break;
                        default:
                            throw new ArgumentException($"Unknown index: {Res}");
                    }

                    break;
            }

            Start();
        }

        private void BuyItem(Dictionary<string, string> item, int index, bool isBodyPart)
        {
            int CorrectPrice = int.Parse(item["price"]);

            if (CorrectPrice > Player.Money)
            {
                Console.WriteLine("You don't have enough money to buy it.");
                return;
            }

            string Res;

            if (item["type"] == "armor" || item["type"] == "damage")
                Res = AskQuestion.Main($"Are you sure you want to buy the {item["name"]} with {item["type"]} {item["units"]} for {item["bodyPart"]}, available items: {item["availableItems"]}, price: {item["price"]} (you have {Player.Money} money)? Y/N;", "Y", "N");
            else
                Res = AskQuestion.Main($"Are you sure you want to buy the {item["name"]} with {item["type"]} {item["units"]}, available items: {item["availableItems"]}, price: {item["price"]} (you have {Player.Money} money)? Y/N;", "Y", "N");

            if (Res == "N")
                return;

            Player.ModifyMoney(-CorrectPrice);

            Console.WriteLine("You bought the item.");

            if (isBodyPart)
            {
                if (item["availableItems"] == "1")
                    _clothesItems = _clothesItems.Where((item, indexItem) => indexItem != index).ToArray();
                else
                    _healItems[index]["availableItems"] = (int.Parse(item["availableItems"]) - 1).ToString();
            }
            else if (item["type"] == "heal")
            {
                if (item["availableItems"] == "1")
                    _healItems = _healItems.Where((item, indexItem) => indexItem != index).ToArray();
                else
                    _healItems[index]["availableItems"] = (int.Parse(item["availableItems"]) - 1).ToString();

            }

            string Name = item["name"];
            string Type = item["type"];
            int Units = int.Parse(item["units"]);

            Res = AskQuestion.Main($"What do you want to do with the item?\n{(isBodyPart ? "Equip" : "Use")} the item: P;\nPut the item in inventory: I;", "P", "I");

            switch (Res)
            {
                case "I":
                    if (isBodyPart)
                        _inventory.AddNewItem(item["name"], Type, Units, FindBodyPartByStr.Main(item["bodyPart"]));
                    else
                        _inventory.AddNewItem(Name, Type, Units, int.Parse(item["unitsForSale"]));

                    Console.WriteLine("Item added to inventory.");
                    break;
                case "P":
                    if (isBodyPart)
                    {
                        _inventory.AddNewItem(Name, Type, Units, FindBodyPartByStr.Main(item["bodyPart"]));

                        List<Dictionary<string, string>> arr;

                        arr = FindBodyPartByStr.Main(item["bodyPart"]) != BodyPart.Weapon ? _inventory.Armor : _inventory.WeaponInventory;

                        _inventory.EquipClothes(arr.ElementAt(arr.Count - 1), arr.Count - 1);
                    }
                    else
                    {
                        _inventory.AddNewItem(Name, Type, Units, int.Parse(item["unitsForSale"]));
                        _inventory.UseItem(_inventory.Other[_inventory.Other.Count - 1]);
                    }
                    break;
            }
        }

        private void ShowOneCategory(string nameCategory, char symbol, Dictionary<string, string>[] arr)
        {
            Console.WriteLine($"  {nameCategory} items:");

            if (arr.Length == 0)
            {
                Console.WriteLine($"    {nameCategory} category is empty");
                return;
            }

            for (int i = 0; i < arr.Length; i++)
            {
                string Name = arr[i]["name"];
                string Units = arr[i]["units"];
                string Price = arr[i]["price"];
                string Type = arr[i]["type"];
                string AvailableItems = arr[i]["availableItems"];

                if (nameCategory == "Clothes")
                {
                    string BodyPart = arr[i]["bodyPart"];

                    Console.WriteLine($"    {symbol}{i + 1}: {Name} has {Type} {Units} for {BodyPart}, available items: {AvailableItems}, price: {Price};");
                }
                else
                    Console.WriteLine($"    {symbol}{i + 1}: {Name} has {Type} {Units}, available items: {AvailableItems}, price: {Price};");
            }
        }

        private Dictionary<string, string> GetNewClothes(string type, BodyPart bodyPart)
        {
            int Units;
            int MaxValueForRandom = 10 + (Player.Level < 5 ? 0 : Player.Level - 5);

            int RandomItem = _random.Next(1, MaxValueForRandom);

            if (RandomItem <= 4)
                Units = 2;
            else if (RandomItem <= 7)
                Units = 3;
            else
                Units = 4;

            Units += Player.Level >= 5 ? Player.Level - 5 : 0;

            int Price = GetPrice(Units);

            return new Dictionary<string, string> {
                { "name", NamesItem.GetName(bodyPart.ToString(), Units - 1) },
                { "units", Units.ToString() },
                { "bodyPart", bodyPart.ToString() },
                { "type", type },
                { "price", Price.ToString()  },
                { "availableItems", "1"}
            };
        }

        private int GetPrice(int unitsItem)
        {
            int Price = unitsItem * 100;

            if (Player.Level > 6)
                Price += (Player.Level - 5) * 5;

            return Price;
        }

        public void AddToArrIndexes(char symbol, int arrCount, ref List<string> arrIndexes)
        {
            for (int i = 1; i <= arrCount; i++)
            {
                arrIndexes.Add($"{symbol}{i}");
            }
        }
    }
}
