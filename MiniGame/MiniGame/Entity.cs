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
        public readonly int StartHealth;
        //public int Armor { get; protected set; }
        public int Damage { get; protected set; }

        private Random random = new Random();

        public Entity(int health, /*int armor,*/ int damage)
        {
            StartHealth = health;
            Health = health;
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
            int Different = StartHealth - Health;

            if (Different < heal)
                heal = Different;

            Health += heal;
        }

        public string GetHealth() 
        {
            return $"{Health}/{StartHealth}";
        }

        public int AttackSomebody()
        {
            return Damage + random.Next(-3, 4);
        }
    }
}
