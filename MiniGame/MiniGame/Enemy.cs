using MiniGame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    internal class Enemy : Entity
    {
        private DifficultEnemies _difficulty;

        public Enemy(DifficultEnemies difficulty, int health, int damage, int armor) : base(health, armor, damage)
        {
            _difficulty = difficulty;
        }

        public override void ShowStatistics()
        {
            Console.WriteLine($"Opponent statistic: \nHealth: {base.GetHealth()} \nArmor: {Armor} \nDamage: {Damage} \nDifficulty: {_difficulty}");
        }
    }
}
