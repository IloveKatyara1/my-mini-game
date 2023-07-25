using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    internal class Entity
    {
        public int Health { get; protected set; }
        //public int Armor { get; protected set; }
        public int Damage { get; protected set; }

        public Entity(int heath, /*int armor,*/ int damage)
        {
            Health = heath;
            //Armor = armor;
            Damage = damage;
        }

        public void TakeDamage(int damage)
        {
            //damage -= Armor;

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

        public void ShowStatistic()
        {
            Console.WriteLine($"Health: {Health} / 100 \nArmor: {/* Armor */ 0} \nDamage: {Damage}");
        }
    }
}
