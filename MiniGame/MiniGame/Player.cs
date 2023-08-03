using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    internal class Player : Entity
    {
        private int _completedRooms = 0;

        public Player() : base (100, 10)
        {
        }
         
        public override void ShowStatistic()
        {
            Console.WriteLine($"Your statistic: \nHealth: {base.GetHealth()}\nArmor: {/* Armor */ 0} \nDamage: {Damage}\nCompleted rooms: {_completedRooms}");
        }

        public void AddNewCompletedRoom() => _completedRooms++;
    }
}
