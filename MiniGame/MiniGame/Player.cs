using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    internal class Player : Entity
    {
        public Player() : base (100, 10)
        {
        }

        //public void ModifyArmor(int amount)
        //{
        //    Armor += amount;
        //}

        //public void ModifyDamage(int amount)
        //{
        //    Damage += amount;
        //}

        public void ShowStatistic()
        {
            Console.WriteLine($"Your statistic: \nHealth: {base.GetHealth()}\nArmor: {/* Armor */ 0} \nDamage: {Damage}");
        }
    }
}
