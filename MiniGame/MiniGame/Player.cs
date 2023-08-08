using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    internal class  Player : Entity
    {
        private int _completedRooms = 0;
        private int _needCompleteRoomsForNextLvl = 1;
        public int Lvl { get; private set; } = 1;
        public Player() : base (100,0, 10)
        {
        }

        public void ModifyArmor(int armor)
        {
            base.Armor += armor;
        }

        public void ModifyDamage(int damage)
        {
            base.Damage += damage;
        }

        public override void ShowStatistic()
        {
            Console.WriteLine($"Your statistic: \nHealth: {base.GetHealth()}\nArmor: {Armor} \nDamage: {Damage}\nCompleted rooms: {_completedRooms}");
        }

        public void AddNewCompletedRoom()
        {
            _completedRooms++;

            if(_needCompleteRoomsForNextLvl == _completedRooms)
            {
                _needCompleteRoomsForNextLvl *= 3;

                Console.WriteLine($"You upgraded your level, now it's {++Lvl}, need to complete {_needCompleteRoomsForNextLvl - _completedRooms} rooms for the next upgrade.");

                base.StartHealth++;
                base.Health = base.StartHealth;
                base.Armor++;
                base.Damage++;
            }
        }
    }
}
