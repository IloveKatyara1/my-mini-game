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
        private readonly NamesItem _names = new();
        private readonly Random _random = new Random();
        private readonly AskQuestion _ask = new();
        private readonly FindBodyPartByStr _findBodyPartByStr = new();
        private readonly Inventory _inventory;
        private readonly Player _player;

        private Dictionary<string, string>[] _clothestItems;
        private Dictionary<string, string>[] _healItems;

        public Seller(Player player, Inventory inv) 
        {
            _inventory = inv;
            _player = player;

            _clothestItems = new Dictionary<string, string>[]
            {
                GetNewClothest("armor", BodyPart.Helmet),
                GetNewClothest("armor", BodyPart.Body),
                GetNewClothest("armor", BodyPart.Legs),
                GetNewClothest("armor", BodyPart.Shield),
                GetNewClothest("damage", BodyPart.Weapon)
            };

            _healItems = new Dictionary<string, string>[]
            {
                new Dictionary<string, string> {
                    { "name", _names.GetName("Heal", 0) },
                    { "units", "20" },
                    { "type", "heal" },
                    { "price", GetPrice(1).ToString() },
                    { "unitsForSale", "1" },
                    { "avaibleItems", "3"}
                },
                new Dictionary<string, string> {
                    { "name", _names.GetName("Heal", 1) },
                    { "units", "50" },
                    { "type", "heal" },
                    { "price", GetPrice(2).ToString() },
                    { "unitsForSale", "2" },
                    { "avaibleItems", "2"}
                },
                new Dictionary<string, string> {
                    { "name", _names.GetName("Heal", 2) },
                    { "units", "100" },
                    { "type", "heal" },
                    { "price", GetPrice(3).ToString() },
                    { "unitsForSale", "3" },
                    { "avaibleItems", "1"}
                },
                new Dictionary<string, string> {
                    { "name", _names.GetName("Heal", 3) },
                    { "units", "999" },
                    { "type", "heal" },
                    { "price", GetPrice(4).ToString() },
                    { "unitsForSale", "4" },
                    { "avaibleItems", "1"}
                }
            };

            Console.WriteLine("You found a saller\nThe saller showed him selling items");
        }

        public void Start()
        {
            Console.WriteLine("Selling items:");

            ShoweOneCategory("Clothest", 'C', _clothestItems);
            ShoweOneCategory("Heal items", 'H', _healItems);

            List<string> ArrIndex = new List<string>() { "E", "I", "S", "L" };

            AddToArrIndexes('C', _clothestItems.Length, ref ArrIndex);
            AddToArrIndexes('H', _healItems.Length, ref ArrIndex);

            string Res = _ask.AskQuestionMain("Would you like to buy something, write the index of item;\nto look your inventory: I;\nto sell something for saller: L;\nto look your statistics: S;\nwrite E for exit", ArrIndex.ToArray());

            if (Res == "E")
                return;

            switch(Res)
            {
                case "E":
                    return;
                case "I":
                    _inventory.ShowInventory();
                    break;
                case "S":
                    _player.ShowStatistic();
                    break;
                case "L":
                    _inventory.ShowInventory(isSeller: true);
                    break;
                default:
                    int CurrentIndex = int.Parse(Res.Substring(1)) - 1;

                    switch (Res[0])
                    {
                        case 'C':
                            BuyItem(_clothestItems[CurrentIndex], CurrentIndex, isBodyPart: true);
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

            if (CorrectPrice > _player.Money)
            {
                Console.WriteLine("You don't have enough money, to buy it");
                return;
            }

            string Res;

            if (item["type"] == "armor" || item["type"] == "damage")
                Res = _ask.AskQuestionMain($"Are you sure want to buy the, {item["name"]} have {item["type"]} {item["units"]} for {item["bodyPart"]}, avaible items {item["avaibleItems"]}, price {item["price"]} (you have {_player.Money} money), Y/N;", "Y", "N");
            else
                Res = _ask.AskQuestionMain($"Are you sure want to buy the, {item["name"]} have {item["type"]} {item["units"]}, avaible items {item["avaibleItems"]}, price {item["price"]} (you have {_player.Money} money), Y/N;", "Y", "N");

            if (Res == "N")
                return;

            _player.ModifyMoney(-CorrectPrice);

            Console.WriteLine("You bought the item");

            if(isBodyPart)
            {
                if (item["avaibleItems"] == "1")
                    _clothestItems = _clothestItems.Where((item, indexItem) => indexItem != index).ToArray();
                else
                    _healItems[index]["avaibleItems"] = (int.Parse(item["avaibleItems"]) - 1).ToString();
            } 
            else if (item["type"] == "heal")
            {
                if (item["avaibleItems"] == "1")
                    _healItems = _healItems.Where((item, indexItem) => indexItem != index).ToArray();
                else
                    _healItems[index]["avaibleItems"] = (int.Parse(item["avaibleItems"]) - 1).ToString();

            }

            string Name = item["name"];
            string Type = item["type"];
            int Units = int.Parse(item["units"]);

            Res = _ask.AskQuestionMain($"What do you want to do with the item?\n{(isBodyPart ? "Equip" : "Use")} the item: P;\nPut the item in inventory: I;", "P", "I");

            switch (Res)
            {
                case "I":
                    if (isBodyPart)
                        _inventory.AddNewItem(item["name"], Type, Units, _findBodyPartByStr.Main(item["bodyPart"]));
                    else
                        _inventory.AddNewItem(Name, Type, Units, int.Parse(item["unitsForSale"]));

                    Console.WriteLine("Item added to inventory");
                    break;
                case "P":
                    if (isBodyPart)
                    {
                        _inventory.AddNewItem(Name, Type, Units, _findBodyPartByStr.Main(item["bodyPart"]));

                        List<Dictionary<string, string>> arr;

                        arr = _findBodyPartByStr.Main(item["bodyPart"]) != BodyPart.Weapon ? _inventory.Armor : _inventory.WeaponInventory;

                        _inventory.EquipClothest(arr.ElementAt(arr.Count - 1), arr.Count - 1);
                    }
                    else
                    {
                        _inventory.AddNewItem(Name, Type, Units, int.Parse(item["unitsForSale"]));
                        _inventory.UseItem(_inventory.Other[_inventory.Other.Count - 1]);
                    }
                    break;
            }
        }

        private void ShoweOneCategory(string nameCategory, char symbol, Dictionary<string, string>[] arr)
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
                string AvaibleItems = arr[i]["avaibleItems"];

                if (nameCategory == "Clothest")
                {
                    string BodyPart = arr[i]["bodyPart"];

                    Console.WriteLine($"    {symbol}{i + 1}: {Name} have {Type} {Units} for {BodyPart}, avaible items {AvaibleItems}, price {Price};");
                } 
                else
                    Console.WriteLine($"    {symbol}{i + 1}: {Name} have {Type} {Units}, avaible items {AvaibleItems}, price {Price};");
            }
        }

        private Dictionary<string, string> GetNewClothest(string type, BodyPart bodyPart)
        {
            int Units;
            int MaxValueForRandom = 10 + (_player.Lvl < 5 ? 0 : _player.Lvl - 5);

            int RandomItem = _random.Next(1, MaxValueForRandom);

            if (RandomItem <= 4)
                Units = 2;
            else if (RandomItem <= 7)
                Units = 3;
            else
                Units = 4;

            Units += _player.Lvl >= 5 ? _player.Lvl - 5 : 0;

            int Price = GetPrice(Units);

            return new Dictionary<string, string> { 
                { "name", _names.GetName(bodyPart.ToString(), Units - 1) }, 
                { "units", Units.ToString() },
                { "bodyPart", bodyPart.ToString() },
                { "type", type }, 
                { "price", Price.ToString()  },
                { "avaibleItems", "1"}
            };
        }

        private int GetPrice(int unitsItem)
        {
            int Price = unitsItem * 100;


            if (_player.Lvl > 6)
                Price += (_player.Lvl - 5) * 5;

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
