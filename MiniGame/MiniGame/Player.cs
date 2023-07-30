using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    internal class Player : Entity
    {
        private List<Dictionary<string, Dictionary<string, int>>> _inventory = new List<Dictionary<string, Dictionary<string, int>>>();

        public Player() : base (100, 10)
        {
            AddNewItemToInventory("eeee1", 1);
            AddNewItemToInventory("eeee12", 2);
            AddNewItemToInventory("eeee123", 3);
        }

        //public void ModifyArmor(int amount)
        //{
        //    Armor += amount;
        //}

        //public void ModifyDamage(int amount)
        //{
        //    Damage += amount;
        //}
                
        public void AddNewItemToInventory(string name, int armor)
        {
            var _item = new Dictionary<string, Dictionary<string, int>>
            {
                { name, new Dictionary<string, int> { { "armor", armor } } }
            };

            _inventory.Add(_item);
        }
        public override void ShowStatistic()
        {
            Console.WriteLine($"Your statistic: \nHealth: {base.GetHealth()}\nArmor: {/* Armor */ 0} \nDamage: {Damage}");
        }

        public int ShowInventory()
        {
            if(_inventory.Count == 0)
            {
                Console.WriteLine("Inventory is empty");
                return 0;
            }

            Console.WriteLine($"\nInventory:");

            for (int i = 0; i < _inventory.Count; i++)
            {
                foreach (var entry in _inventory[i])
                {
                    string Name = entry.Key;
                    int Armor = entry.Value["armor"];
                    Console.Write($"{i + 1}: {Name} have armor {Armor}; \n");
                }
            }

            return _inventory.Count;
        }

        public Dictionary<string, Dictionary<string, int>> GetItemByNum(int num)
        {
            return _inventory[num - 1];
        }
    }
}
