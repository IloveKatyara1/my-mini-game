using MiniGame.Untils;
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
        public List<Dictionary<string, string>> Weapon { get; private set; } = new List<Dictionary<string, string>>();
        public Dictionary<string, Dictionary<string, string>> Equipped { get; private set; } = new Dictionary<string, Dictionary<string, string>>()
        {
            {BodyPart.Helmet.ToString(), new Dictionary<string, string> { {"name", "null" }, { "units", "null" }, { "type", "armor" } } },
            {BodyPart.Body.ToString(), new Dictionary<string, string> { { "name", "null" }, { "units", "null" }, { "type", "armor" } } },
            {BodyPart.Legs.ToString(), new Dictionary<string, string> { { "name", "null" }, { "units", "null" }, { "type", "armor" } } },
            {BodyPart.LHand.ToString(), new Dictionary<string, string> { { "name", "null" }, { "units", "null" }, { "type", "damage" } } }
        };

        public Inventory(Player player)
        {
            _player = player;

            AddNewItem("armor1", "armor", "1", BodyPart.Helmet);
            AddNewItem("Devil Helmet", "armor", "3", BodyPart.Helmet);
            AddNewItem("armor2", "armor", "2", BodyPart.Body);
            AddNewItem("armor3", "armor", "3", BodyPart.Legs);
            AddNewItem("damage1", "damage", "1", BodyPart.LHand);
            AddNewItem("damage2", "damage", "2", BodyPart.LHand);
        }

        public void AddNewItem(string name, string type, string units, BodyPart bodyPart)
        {
            var newItem = new Dictionary<string, string>() { { "name", name }, { "units", units }, { "bodyPart", bodyPart.ToString() }, { "type", type } };

            switch (type)
            {
                case "armor":
                    Armor.Add(newItem);
                    break;
                case "damage":
                    Weapon.Add(newItem);
                    break;
            }
        }

        public void ShowInventory()
        {
            Console.WriteLine($"\nYour Inventory:");
            ShowEquippedItems();
            ShowOneCategory(Armor, "armor", 'A');
            ShowOneCategory(Weapon, "damage", 'W');

            FindIndexAndArr();
        }

        private void ShowEquippedItems()
        {
            int i = 1;

            Console.WriteLine($"  equipped items:");

            foreach (var category in Equipped)
            {
                string categoryName = category.Key;
                Dictionary<string, string> itemInfo = category.Value;

                Console.WriteLine($"    E{i}: {categoryName} equipped {itemInfo["name"]} have {itemInfo["type"]} {itemInfo["units"]}");

                i++;
            }
        }

        private void ShowOneCategory(List<Dictionary<string, string>> category, string name, char symbol)
        {
            Console.WriteLine($"  {name} items:");

            if (category.Count == 0)
            {
                Console.WriteLine($"    {char.ToUpper(name[0]) + name.Substring(1)} category is empty");
                return;
            }

            for (int i = 0; i < category.Count; i++)
            {
                string Name = category[i]["name"];
                string Units = category[i]["units"];
                string BodyPart = category[i]["bodyPart"];

                Console.Write($"    {symbol}{i + 1}: {Name} have {name} {Units} for {BodyPart}; \n");

            }
        }
        
        public void FindIndexAndArr()
        {
            List<string> Indexes = new List<string>();

            AddToArrIndexes('A', Armor.Count, ref Indexes);
            AddToArrIndexes('W', Weapon.Count, ref Indexes);
            AddToArrIndexes('E', Equipped.Count, ref Indexes);

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
                    ChooseTheItem(Res, Armor);
                    break;
                case 'W':
                    ChooseTheItem(Res, Weapon);
                    break;
                default:
                    Console.WriteLine("Default");
                    break;
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
                    AddNewItem(item["name"], item["type"], item["units"], FindCategoryByString(itemKey));

                    MakeDefaultEquipped(itemKey, item);
                    break;
                case "T":
                    MakeDefaultEquipped(itemKey, item);
                    break;
                case "B":
                    ShowInventory();
                    break;
            }
        }

        private void MakeDefaultEquipped(string itemKey, Dictionary<string, string> item)
        {
            FindWhatNeedsModification(item["type"], -int.Parse(item["units"]));

            if (itemKey != BodyPart.LHand.ToString())
                Equipped[itemKey] = new Dictionary<string, string> { { "name", "null" }, { "units", "null" }, { "type", "armor" } };
            else
                Equipped[BodyPart.LHand.ToString()] = new Dictionary<string, string> { { "name", "null" }, { "units", "null" }, { "type", "damage" } };

            ShowInventory();
        }

        private void AddToArrIndexes(char symbol, int arrCount, ref List<string> arrIndexes)
        {
            for (int i = 1; i <= arrCount; i++)
            {
                arrIndexes.Add($"{symbol}{i}");
            }
        }

        private void ChooseTheItem(string index, List<Dictionary<string, string>> arr)
        {
            int numIndex = int.Parse(index.Substring(1)) - 1;
            Dictionary<string, string> item = arr[numIndex];

            Console.WriteLine($"You selected: {item["name"]}, {item["type"]}: {item["units"]}");

            string ResSecond = _askQuestion.AskQuestionMain($"What do you want to do whith this item \nEquip the item: P; \nThrow away the item: T;\nBack to all items: B;\nExit: E", "P", "T", "B", "E");

            switch (ResSecond)
            {
                case "P":
                    if (Equipped[item["bodyPart"]]["name"] != "null")
                    {
                        Console.WriteLine($"You heve equipped {item["name"]} have {item["type"]} {item["units"]}. Do you want change it?");
                        string Res = _askQuestion.AskQuestionMain($"Y/N", "Y", "N");

                        if (Res == "N")
                            return;

                        var equippedItem = Equipped[item["bodyPart"]];

                        AddNewItem(equippedItem["name"], equippedItem["type"], equippedItem["units"], FindCategoryByString(item["bodyPart"]));

                        FindWhatNeedsModification(equippedItem["type"], -int.Parse(equippedItem["units"]));
                    }

                    Equipped[item["bodyPart"]] = new Dictionary<string, string>
                    {
                        { "name", item["name"] },
                        { "units", item["units"] },
                        { "type", item["type"] }
                    };

                    RemoveIndex(item["type"], numIndex);

                    FindWhatNeedsModification(item["type"], int.Parse(item["units"]));

                    ShowInventory();
                    break;
                case "T":
                    RemoveIndex(item["type"], numIndex);
                    Console.WriteLine("The item was removed");
                    break;
                case "B":
                    ShowInventory();
                    break;
                case "E":
                    return;
            }
        }

        private void RemoveIndex(string type, int index) 
        {
            switch (type)
            {
                case "armor":
                    Armor.RemoveAt(index);
                    break;
                case "damage":
                    Weapon.RemoveAt(index);
                    break;
            }
        }

        private BodyPart FindCategoryByString(string bodyPart)
        {
            return bodyPart switch
            {
                "Helmet" => BodyPart.Helmet,
                "Body" => BodyPart.Body,
                "Legs" => BodyPart.Legs,
                "LHand" => BodyPart.LHand,
                //case "RHand":
                //    return BodyPart.RHand;
                _ => BodyPart.Body,
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
            }
        }
    }
}
