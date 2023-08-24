using MiniGame.Enums;
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
        private bool _isSeller = false;
        private Player _player;

        public List<Dictionary<string, string>> Armor { get; private set; } = new List<Dictionary<string, string>>();
        public List<Dictionary<string, string>> Weapon_inventory { get; private set; } = new List<Dictionary<string, string>>();
        public List<Dictionary<string, string>> Other { get; private set; } = new List<Dictionary<string, string>>()
        {
            new Dictionary<string, string>() { { "name", "Small healing flask" }, { "units", "20" }, { "type", "heal" }, { "unitsForSale", "1" } },
            new Dictionary<string, string>() { { "name", "Medium healing flask" }, { "units", "50" }, { "type", "heal" }, { "unitsForSale", "2" } },
            new Dictionary<string, string>() { { "name", "Big healing flask" }, { "units", "100" }, { "type", "heal" }, { "unitsForSale", "3" } },
        };
        public Dictionary<BodyPart, Dictionary<string, string>> Equipped { get; private set; } = new Dictionary<BodyPart, Dictionary<string, string>>()
        {
            {BodyPart.Helmet, new Dictionary<string, string> { {"name", "null" }, { "units", "null" }, { "type", "armor" } } },
            {BodyPart.Body, new Dictionary<string, string> { { "name", "null" }, { "units", "null" }, { "type", "armor" } } },
            {BodyPart.Legs, new Dictionary<string, string> { { "name", "Metallic panties" }, { "units", "1" }, { "type", "armor" }, { "unitsForSale", "1" } } },
            {BodyPart.Shield, new Dictionary<string, string> { { "name", "null" }, { "units", "null" }, { "type", "armor" } } },
            {BodyPart.Weapon, new Dictionary<string, string> { { "name", "Toothpick" }, { "units", "1" }, { "type", "damage" }, { "unitsForSale", "1" } } }
        };

        public _inventory(Player player)
        {
            _player = player;
        }

        public void AddNewItem(string name, string type, int units, BodyPart bodyPart)
        {
            var newItem = new Dictionary<string, string>() 
            { 
                { "name", name }, 
                { "units", units.ToString() }, 
                { "bodyPart", bodyPart.ToString() }, 
                { "type", type }, 
                { "unitsForSale", units.ToString() } 
            };

            switch (type)
            {
                case "armor":
                    Armor.Add(newItem);
                    break;
                case "damage":
                    Weapon_inventory.Add(newItem);
                    break;
            }
        }

        public void AddNewItem(string name, string type, int units, int unitsForSale)
        {
            var newItem = new Dictionary<string, string>() { { "name", name }, { "units", units.ToString() }, { "type", type }, { "unitsForSale", unitsForSale.ToString() } };

            Other.Add(newItem);
        }

        public void Show_inventory(bool isSeller = false)
        {
            _isSeller = isSeller;

            Console.WriteLine($"\nYour _inventory:");
            ShowEquippedItems(Equipped);
            ShowOneCategory(Armor, "armor", 'A');
            ShowOneCategory(Weapon_inventory, "damage", 'W');
            ShowOneCategory(Other, "other", 'O');

            ChooseItem();
        }

        private void ShowEquippedItems(Dictionary<BodyPart, Dictionary<string, string>>  arr)
        {
            int i = 1;

            Console.WriteLine($"  Equipped items:");

            foreach (var category in arr)
            {
                BodyPart categoryName = category.Key;
                Dictionary<string, string> itemInfo = category.Value;

                string PriceItem = "";

                if (itemInfo["units"] != "null" && _isSeller)
                    PriceItem = $", price: {GetPriceItem(int.Parse(itemInfo["unitsForSale"]))}";

                string ShowStr = $"    E{i}: {categoryName} equipped {itemInfo["name"]} have {itemInfo["type"]} {itemInfo["units"]}{PriceItem};";

                Console.WriteLine(ShowStr);

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

                    Console.WriteLine($"    {symbol}{i + 1}: {Name} have {name} {Units} for {BodyPart}" +
                        $"{(_isSeller ? $", price: {GetPriceItem(int.Parse(category[i]["unitsForSale"]))}" : "")};");
                }
                else
                    Console.WriteLine($"    {symbol}{i + 1}: {Name} have {category[i]["type"]} {Units}" +
                        $"{(_isSeller ? $", price: {GetPriceItem(int.Parse(category[i]["unitsForSale"]))}" : "")};");
            }
        }
        
        public void ChooseItem()
        {
            List<string> Indexes = new List<string>() { "E" };

            AddToArrIndexes('A', Armor.Count, ref Indexes);
            AddToArrIndexes('W', Weapon_inventory.Count, ref Indexes);
            AddToArrIndexes('E', Equipped.Count, ref Indexes);
            AddToArrIndexes('O', Other.Count, ref Indexes);

            string Res = AskQuestion.Main($"Select item by type number, or exit: E", Indexes.ToArray());

            if (Res == "E")
                return;

            int CurrentIndex = int.Parse(Res.Substring(1)) - 1;

            switch (Res[0])
            {
                case 'E':
                    ChooseActianForEquipped(CurrentIndex);
                    break;
                case 'A':
                    ChooseActianForItem(CurrentIndex, Armor);
                    break;
                case 'W':
                    ChooseActianForItem(CurrentIndex, Weapon_inventory);
                    break;
                case 'O':
                    ChooseActianForItem(CurrentIndex, Other, isClothes: false);
                    break;
                default:
                    throw new ArgumentException($"Unknown index: {Res[0]}");
            }
        }

        private void ChooseActianForEquipped(int index)
        {
            var itemKey = Equipped.ElementAt(index).Key;
            var item = Equipped[itemKey];

            if (item["name"] == "null")
            {
                Console.WriteLine("First you need to equip something");
                Show_inventory(_isSeller);
                return;
            }

            Console.WriteLine($"You selected: {item["name"]}, {item["type"]}: {item["units"]}");

            string[] CharsForQuestion = new string[]{ "P", "T", "B", "E" };

            if (_isSeller)
                CharsForQuestion = new string[] { "P", "T", "B", "E", "S" };

            string Res = AskQuestion.Main($"What do you want to do whith this item " +
                $"\nUnequip the item : P;" +
                $"\nThrow away the item: T;" +
                $"\nBack to all items: B;" +
                $"{(_isSeller ? $"\nSell the item: S; (price of the item:{GetPriceItem(int.Parse(item["unitsForSale"]))})" : "")}" +
                $"\nExit: E", 
                CharsForQuestion);

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
                case "S":
                    SellItem(item, itemKey.ToString());
                    MakeDefaultEquipped(itemKey, item);
                    break;
            }

            Show_inventory(_isSeller);
        }

        private void MakeDefaultEquipped(BodyPart itemKey, Dictionary<string, string> item)
        {
            FindWhatNeedsModification(item["type"], -int.Parse(item["units"]));

            if (itemKey != BodyPart.Weapon)
                Equipped[itemKey] = new Dictionary<string, string> { { "name", "null" }, { "units", "null" }, { "type", "armor" } };
            else
                Equipped[BodyPart.Weapon] = new Dictionary<string, string> { { "name", "null" }, { "units", "null" }, { "type", "damage" } };
        }

        public void AddToArrIndexes(char symbol, int arrCount, ref List<string> arrIndexes)
        {
            for (int i = 1; i <= arrCount; i++)
            {
                arrIndexes.Add($"{symbol}{i}");
            }
        }

        private void ChooseActianForItem(int index, List<Dictionary<string, string>> arr, bool isClothes = true)
        {
            Dictionary<string, string> item = arr[index];

            string[] CharsForQuestion = new string[] { "P", "T", "B", "E" };

            if (_isSeller)
                CharsForQuestion = new string[] { "P", "T", "B", "E", "S" };

            Console.WriteLine($"You selected: {item["name"]}, {item["type"]}: {item["units"]}");

            string ResSecond = AskQuestion.Main($"What do you want to do with this item?" +
                $"\n{(isClothes ? "Equip" : "Use")} the item: P;" +
                $"\nThrow away the item: T;" +
                $"\nBack to all items: B;" +
                $"{(_isSeller ? $"\nSell the item: S; (price of the item: {GetPriceItem(int.Parse(item["unitsForSale"]))})" : "")}" +
                $"\nExit: E",
                CharsForQuestion);

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
                case "S":
                    if (isClothes)
                        SellItem(item, item["bodyPart"]);
                    else
                        SellItem(item, null);
                    RemoveIndex(item["type"], index);
                    break;
                case "E":
                    return;
            }

            Show_inventory(_isSeller);
        }

        private void SellItem(Dictionary<string, string> item, string? bodyPart)
        {
            int PriceItem = GetPriceItem(int.Parse(item["unitsForSale"]));

            string Res = AskQuestion.Main(
            $"Are you sure want to sell the {item["name"]}, {item["type"]}: {item["units"]}{(bodyPart != null ? $" for {bodyPart}" : "")}. You will receive {PriceItem} money. Y/N",
            "Y", "N");

            if (Res == "N")
                return;

            _player.ModifyMoney(PriceItem);

            Console.WriteLine($"You sold the item, and received {PriceItem} money");
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
            BodyPart CurrentBodyPart = FindBodyPartByStr.Main(item["bodyPart"]);

            if (Equipped[CurrentBodyPart]["name"] != "null")
            {
                Console.WriteLine($"You already have the equipped {Equipped[CurrentBodyPart]["name"]} have {Equipped[CurrentBodyPart]["type"]} {Equipped[CurrentBodyPart]["units"]}, do you want to change it to {item["name"]} have {item["type"]} {item["units"]}?");
                string Res = AskQuestion.Main($"Y/N", "Y", "N");

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
                { "type", item["type"] },
                { "unitsForSale", item["unitsForSale"] }
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
                    Weapon_inventory.RemoveAt(index);
                    break;
                default:
                    Other.RemoveAt(index);
                    break;
            }
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

        private int GetPriceItem(int units) => units * 100 / 4;
    }
}
