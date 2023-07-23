using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    internal class Player
    {
        private int health = 100;
        private int armor = 0;
        private int damage = 10;

        public void TakeDamage(int damage)
        {
            damage -= armor;

            if (damage <= 0)
               damage = 0;
            else if (damage > health)
               damage = health;

            health -= damage;
        }

        public void HealHealth(int heal)
        {
            int Different = 100 - health;

            if (Different < heal)
               heal = Different;

            health += heal;
        }

        public void ShowStatistic()
        {
            Console.WriteLine($"Health: {health} / 100 \nArmor: {armor} \nDamage: {damage}");
        }

        public int ShowDamage()
        {
            return damage;
        }
    }
}
