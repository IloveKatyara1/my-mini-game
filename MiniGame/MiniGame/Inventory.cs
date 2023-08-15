﻿using MiniGame.Untils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MiniGame
{
    internal class Inventory
    {
        private readonly AskQuestion _askQuestion = new();
        private readonly Player _player;

        public List<Dictionary<string, string>> Armor { get; private set; } = new List<Dictionary<string, string>>();
        public List<Dictionary<string, string>> WeaponInventory { get; private set; } = new List<Dictionary<string, string>>();
        public List<Dictionary<string, string>> Other { get; private set; } = new List<Dictionary<string, string>>();
        public Dictionary<BodyPart, Dictionary<string, string>> Equipped { get; private set; } = new Dictionary<BodyPart, Dictionary<string, string>>()
        {
            {BodyPart.Helmet, new Dictionary<string, string> { {"name", "null" }, { "units", "null" }, { "type", "armor" } } },
            {BodyPart.Body, new Dictionary<string, string> { { "name", "null" }, { "units", "null" }, { "type", "armor" } } },
            {BodyPart.Legs, new Dictionary<string, string> { { "name", "null" }, { "units", "null" }, { "type", "armor" } } },
            {BodyPart.Shield, new Dictionary<string, string> { { "name", "null" }, { "units", "null" }, { "type", "armor" } } },
            {BodyPart.Weapon, new Dictionary<string, string> { { "name", "null" }, { "units", "null" }, { "type", "damage" } } }
        };

        public Inventory(Player player)
        {
            _player = player;

            AddNewItem("Metallic panties", "armor", 1, BodyPart.Legs);
            AddNewItem("Toothpick", "damage", 1, BodyPart.Weapon);
            EquipClothest(Armor[0], 0);
            EquipClothest(WeaponInventory[0], 0);

            AddNewItem("Small heal", "heal", 20);
            AddNewItem("Big heal", "heal", 100);
            AddNewItem("Medium heal", "heal", 50);
        }

        public void AddNewItem(string name, string type, int units, BodyPart bodyPart)
        {
            var newItem = new Dictionary<string, string>() { { "name", name }, { "units", units.ToString() }, { "bodyPart", bodyPart.ToString() }, { "type", type } };

            switch (type)
            {
                case "armor":
                    Armor.Add(newItem);
                    break;
                case "damage":
                    WeaponInventory.Add(newItem);
                    break;
            }
        }

        public void AddNewItem(string name, string type, int units)
        {
            var newItem = new Dictionary<string, string>() { { "name", name }, { "units", units.ToString() }, { "type", type } };

            Other.Add(newItem);
        }

        public void ShowInventory()
        {
            Console.WriteLine($"\nYour Inventory:");
            ShowEquippedItems();
            ShowOneCategory(Armor, "armor", 'A');
            ShowOneCategory(WeaponInventory, "damage", 'W');
            ShowOneCategory(Other, "other", 'O');

            ChooseItem();
        }

        private void ShowEquippedItems()
        {
            int i = 1;

            Console.WriteLine($"  equipped items:");

            foreach (var category in Equipped)
            {
                BodyPart categoryName = category.Key;
                Dictionary<string, string> itemInfo = category.Value;

                Console.WriteLine($"    E{i}: {categoryName} equipped {itemInfo["name"]} have {itemInfo["type"]} {itemInfo["units"]}");

                i++;
            }
        }

        private void ShowOneCategory(List<Dictionary<string, string>> category, string name, char symbol)
        {
            Console.WriteLine($"  {char.ToUpper(name[0]) + name.Substring(1)} items:");

            if (category.Count == 0)
            {
                Console.WriteLine($"    {char.ToUpper(name[0]) + name.Substring(1)} category is empty");
                return;
            }

            for (int i = 0; i < category.Count; i++)
            {
                string Name = category[i]["name"];
                string Units = category[i]["units"];

                if(name == "damage" || name == "armor")
                {
                    string BodyPart = category[i]["bodyPart"];

                    Console.WriteLine($"    {symbol}{i + 1}: {Name} have {name} {Units} for {BodyPart};");
                }
                else
                    Console.WriteLine($"    {symbol}{i + 1}: {Name} have {category[i]["type"]} {Units};");
            }
        }
        
        public void ChooseItem()
        {
            List<string> Indexes = new List<string>();

            AddToArrIndexes('A', Armor.Count, ref Indexes);
            AddToArrIndexes('W', WeaponInventory.Count, ref Indexes);
            AddToArrIndexes('E', Equipped.Count, ref Indexes);
            AddToArrIndexes('O', Other.Count, ref Indexes);

            Indexes.Add("E");

            string Res = _askQuestion.AskQuestionMain($"Select item by type number, or exit: E", Indexes.ToArray());

            if (Res == "E")
                return;

            int CurrentIndex = int.Parse(Res.Substring(1)) - 1;

            switch (Res[0])
            {
                case 'E':
                    ChooseActianForEquipped(CurrentIndex);
                    break;
                case 'A':
                    ChooseActianForClothes(CurrentIndex, Armor);
                    break;
                case 'W':
                    ChooseActianForClothes(CurrentIndex, WeaponInventory);
                    break;
                case 'O':
                    ChooseActianForClothes(CurrentIndex, Other, isClothes: false);
                    break;
                default:
                    throw new ArgumentException($"Unknown index: {Res[0]}");
            }
        }

        private void ChooseActianForEquipped(int index)
        {
            var itemKey = Equipped.ElementAt(index).Key;
            var item = Equipped[itemKey];

            Console.WriteLine($"You selected: {item["name"]}, {item["type"]}: {item["units"]}");

            string Res = _askQuestion.AskQuestionMain($"What do you want to do whith this item \nUnequip the item : P; \nThrow away the item: T;\nBack to all items: B;\nExit: E", "P", "T", "B", "E");

            if (Res == "E") return;

            switch(Res) 
            {
                case "E":
                    return;
                case "P":
                    AddNewItem(item["name"], item["type"], int.Parse(item["units"]), itemKey);

                    MakeDefaultEquipped(itemKey, item);
                    break;
                case "T":
                    MakeDefaultEquipped(itemKey, item);
                    break;
            }

            ShowInventory();
        }

        private void MakeDefaultEquipped(BodyPart itemKey, Dictionary<string, string> item)
        {
            FindWhatNeedsModification(item["type"], -int.Parse(item["units"]));

            if (itemKey != BodyPart.Weapon)
                Equipped[itemKey] = new Dictionary<string, string> { { "name", "null" }, { "units", "null" }, { "type", "armor" } };
            else
                Equipped[BodyPart.Weapon] = new Dictionary<string, string> { { "name", "null" }, { "units", "null" }, { "type", "damage" } };
        }

        private void AddToArrIndexes(char symbol, int arrCount, ref List<string> arrIndexes)
        {
            for (int i = 1; i <= arrCount; i++)
            {
                arrIndexes.Add($"{symbol}{i}");
            }
        }

        private void ChooseActianForClothes(int index, List<Dictionary<string, string>> arr, bool isClothes = true)
        {
            Dictionary<string, string> item = arr[index];

            Console.WriteLine($"You selected: {item["name"]}, {item["type"]}: {item["units"]}");

            string ResSecond = _askQuestion.AskQuestionMain($"What do you want to do with this item?\n{(isClothes ? "Equip" : "Use")} the item: P;\nThrow away the item: T;\nBack to all items: B;\nExit: E", "P", "T", "B", "E");

            switch (ResSecond)
            {
                case "P":
                    if (isClothes)
                        EquipClothest(item, index);
                    else
                        UseItem(item);
                    break;
                case "T":
                    RemoveIndex(item["type"], index);
                    Console.WriteLine("The item was removed");
                    break;
                case "E":
                    return;
            }

            ShowInventory();
        }

        public void UseItem(Dictionary<string, string> item)
        {
            if (item["type"] == "heal")
            {
                _player.HealHealth(int.Parse(item["units"]));
                Other.Remove(item);
            }
        }

        public void EquipClothest(Dictionary<string, string> item, int index)
        {
            BodyPart CurrentBodyPart = FindCategoryByString(item["bodyPart"]);

            if (Equipped[CurrentBodyPart]["name"] != "null")
            {
                Console.WriteLine($"You already have the equipped {Equipped[CurrentBodyPart]["name"]} have {Equipped[CurrentBodyPart]["type"]} {Equipped[CurrentBodyPart]["units"]}, do you want to change it to {item["name"]} have {item["type"]} {item["units"]}?");
                string Res = _askQuestion.AskQuestionMain($"Y/N", "Y", "N");

                if (Res == "N")
                    return;

                var equippedItem = Equipped[CurrentBodyPart];

                AddNewItem(equippedItem["name"], equippedItem["type"], int.Parse(equippedItem["units"]), CurrentBodyPart);
                FindWhatNeedsModification(equippedItem["type"], -int.Parse(equippedItem["units"]));
            }

            Equipped[CurrentBodyPart] = new Dictionary<string, string>
            {
                { "name", item["name"] },
                { "units", item["units"] },
                { "type", item["type"] }
            };

            RemoveIndex(item["type"], index);
            FindWhatNeedsModification(item["type"], int.Parse(item["units"]));
        }

        private void RemoveIndex(string type, int index) 
        {
            switch (type)
            {
                case "armor":
                    Armor.RemoveAt(index);
                    break;
                case "damage":
                    WeaponInventory.RemoveAt(index);
                    break;
                default:
                    throw new ArgumentException($"Failed remove. Unknown category: {type}");
            }
        }

        private BodyPart FindCategoryByString(string bodyPart)
        {
            return bodyPart switch
            {
                "Helmet" => BodyPart.Helmet,
                "Body" => BodyPart.Body,
                "Legs" => BodyPart.Legs,
                "Weapon" => BodyPart.Weapon,
                "Shield" => BodyPart.Shield,
                _ => throw new ArgumentException("Invalid body part string"),
            };
        }

        private void FindWhatNeedsModification(string type, int units)
        {
            switch (type)
            {
                case "armor":
                    _player.ModifyArmor(units);
                    break;
                case "damage":
                    _player.ModifyDamage(units);
                    break;
                default:
                    throw new ArgumentException($"Failed modification. Unknown category: {type}");
            }
        }
    }
}
