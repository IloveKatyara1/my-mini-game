using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    internal class  Player : Entity
    {
        public int CompletedRooms { get; private set; } = 0;
        private int _needCompleteRoomsForNextLvl = 1;
        public int Lvl { get; private set; } = 1;
        public int Money { get; private set; } = 0;


        public Player() : base (100,0, 10)
        {
        }

        public void ModifyMoney(int units)
        {
            if (units * -1 > Money)
                throw new ArgumentException("the minus bigger then palyer money");

            Money += units;
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
            Console.WriteLine($"Your statistics:\nHealth: {base.GetHealth()}\nArmor: {Armor}\nDamage: {Damage}\nCompleted rooms: {CompletedRooms}\nYour level: {Lvl}\nRooms need to complete for the next level: {_needCompleteRoomsForNextLvl - CompletedRooms};\nYou have {Money} money"
);
        }

        public void AddNewCompletedRoom()
        {
            CompletedRooms++;

            if(_needCompleteRoomsForNextLvl == CompletedRooms)
            {
                _needCompleteRoomsForNextLvl *= 3;

                Console.WriteLine($"Your level upgraded, now your level is {++Lvl}, you need to complete {_needCompleteRoomsForNextLvl - CompletedRooms} rooms for the next upgrade.");

                base.StartHealth++;
                base.Health = base.StartHealth;
                base.Armor++;
                base.Damage++;
            }
        }
    }
}
