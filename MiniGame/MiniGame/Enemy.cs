using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    internal class Enemy : Entity
    {
        private string _difficulty;

        public Enemy(string difficulty, int health, int damage) : base(health, 0, damage)
        {
            _difficulty = difficulty;
        }

        public override void ShowStatistic()
        {
            Console.WriteLine($"Opponent statistic: \nHealth: {base.GetHealth()} \nArmor: {/* Armor */ 0} \nDamage: {Damage} \nDifficulty: {_difficulty}");
        }
    }
}
