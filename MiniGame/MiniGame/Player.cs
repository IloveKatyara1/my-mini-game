using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    internal class Player
    {
        private int Health = 100;
        private int Armor = 0;
        private int Damage = 10;

        public void TakeDamage(int damage)
        {
            damage -= Armor;

            if (damage <= 0)
               damage = 0;
            else if (damage > Health)
               damage = Health;

            Health -= damage;
        }

        public void HealHealth(int heal)
        {
            int Different = 100 - Health;

            if (Different < heal)
               heal = Different;

            Health += heal;
        }

        public string Statistic()
        {
            return $"Health: {Health} / 100 \nArmor: {Armor} \nDamage: {Damage}";
        }

        public int ShowDamage()
        {
            return Damage;
        }
    }
}
