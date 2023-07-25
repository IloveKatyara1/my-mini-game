using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    internal class Enemy : Entity
    {
        private Random random = new Random();

        private int _health;
        private int _damage;

        public Enemy() : base(0, 0)
        {
            _health = random.Next(80, 121);
            _damage = random.Next(5, 15);

            base.Health = _health;
            base.Damage = _damage;
        }
    }
}
