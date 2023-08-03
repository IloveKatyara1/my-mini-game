using MiniGame.Untils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    internal class Inventory
    {
        private readonly AskQuestion _askQuestion = new();

        public List<Dictionary<string, string>> Armor { get; private set; } = new List<Dictionary<string, string>>();
        public List<Dictionary<string, string>> Weapon { get; private set; } = new List<Dictionary<string, string>>();

        public void AddNewItem(string name, string type, string units) 
        {
            var newItem = new Dictionary<string, string>() { { "name", name }, { type, units } };

            switch(type)
            {
                case "armor":
                    Armor.Add(newItem);
                    break;
                case "damage":
                    Weapon.Add(newItem);
                    break;
            }
        }

        public Inventory()
        {
            AddNewItem("armor1", "armor", "1");
            AddNewItem("armor2", "armor", "2");
            AddNewItem("armor3", "armor", "3");
            AddNewItem("damage1", "damage", "1");
            AddNewItem("damage2", "damage", "2");
        }

        public void ShowInventory()
        {
            Console.WriteLine($"\nYour Inventory:");
            ShowOneCategory(Armor, "armor", 'A');
            ShowOneCategory(Weapon, "damage", 'W');

            FindIndexAndArr();
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
                string Armor = category[i][name];

                Console.Write($"    {symbol}{i + 1}: {Name} have {name} {Armor}; \n");
            }
        }
        
        public void FindIndexAndArr()
        {
            List<string> Indexes = new List<string>();

            AddToArrIndexes('A', Armor.Count, ref Indexes);
            AddToArrIndexes('W', Weapon.Count, ref Indexes);

            Indexes.Add("E");

            string Res = _askQuestion.AskQuestionMain($"Select item by type number, or exit: E", Indexes.ToArray());

            switch(Res[0])
            {
                case 'E':
                    return;
                case 'A':
                    ChooseTheItem(Res, "armor", Armor);
                    break;
                case 'W':
                    ChooseTheItem(Res, "damage", Weapon);
                    break;
                default:
                    Console.WriteLine("Default");
                    break;
            }
        }

        private void AddToArrIndexes(char symbol, int arrCount, ref List<string> arrIndexes)
        {
            for (int i = 1; i <= arrCount; i++)
            {
                arrIndexes.Add($"{symbol}{i}");
            }
        }

        private void ChooseTheItem(string index, string secondLabel, List<Dictionary<string, string>> arr)
        {
            int numIndex = int.Parse(index.Substring(1)) - 1;
            Dictionary<string, string> item = arr[numIndex];

            string itemName = item["name"];
            string armorValue = item[secondLabel];

            Console.WriteLine($"You selected: {itemName}, Armor: {armorValue}");

            string ResSecond = _askQuestion.AskQuestionMain($"What do you want to do whith this item \nPut on the item: P; \nThrow away the item: T;\nBack to all items: B;\nExit: E", "P", "T", "B", "E");

            switch (ResSecond)
            {
                case "P":
                    Console.WriteLine("will soon");
                    break;
                case "T":
                    switch (secondLabel)
                    {
                        case "armor":
                            Armor.RemoveAt(numIndex);
                            break;
                        case "damage":
                            Weapon.RemoveAt(numIndex);
                            break;
                    }
                    Console.WriteLine("The item was removed");
                    break;
                case "B":
                    ShowInventory();
                    break;
                case "E":
                    return;
            }
        }
    }
}
