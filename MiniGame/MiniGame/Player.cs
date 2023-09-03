using System;

namespace MiniGame
{
    internal class Player : Entity
    {
        public int CompletedRooms { get; private set; } = 0;
        private int _needCompletedRoomsForNextLevel = 1;
        public int Level { get; private set; } = 1;
        public int Money { get; private set; } = 0;

        public Player() : base(100, 0, 10)
        {
        }

        public void ModifyMoney(int units)
        {
            if (units * -1 > Money)
                throw new ArgumentException("The negative value is greater than the player's money.");

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

        public override void ShowStatistics()
        {
            Console.WriteLine($"Your statistics:" +
                $"\nHealth: {base.GetHealth()}" +
                $"\nArmor: {Armor}" +
                $"\nDamage: {Damage}" +
                $"\nCompleted rooms: {CompletedRooms}" +
                $"\nYour level: {Level}" +
                $"\nRooms needed to complete for the next level: {_needCompletedRoomsForNextLevel - CompletedRooms};" +
                $"\nYou have {Money} money.");
        }

        public void AddNewCompletedRoom()
        {
            CompletedRooms++;

            if (_needCompletedRoomsForNextLevel == CompletedRooms)
            {
                _needCompletedRoomsForNextLevel *= 3;

                Console.WriteLine($"Your level has been upgraded. Your new level is {++Level}. You need to complete {_needCompletedRoomsForNextLevel - CompletedRooms} rooms for the next upgrade.");

                base.StartHealth++;
                base.Health = base.StartHealth;
                base.Armor++;
                base.Damage++;
            }
        }
    }
}
