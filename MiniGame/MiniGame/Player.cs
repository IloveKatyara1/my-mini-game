using System;

namespace MiniGame
{
    internal class Player : Entity
    {
        public int CompletedRooms { get; private set; } = 0;
        public int NeedCompletedRoomsForNextLevel { get; private set; } = 1;
        public int Level { get; private set; } = 1;
        public int Money { get; private set; } = 0;

        public Player() : base(health: 100, armor: 0, damage: 10)
        {
        }

        public Player(int level, int health, int startHealth, 
                      int armor, int damage, int completedRooms, 
                      int needCompletedRoomsForNextLevel, int money) : base(health, armor, damage, startHealth)
        {
            Level = level;
            CompletedRooms = completedRooms;
            NeedCompletedRoomsForNextLevel = needCompletedRoomsForNextLevel;
            Money = money;
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
                $"\nRooms needed to complete for the next level: {NeedCompletedRoomsForNextLevel - CompletedRooms};" +
                $"\nYou have {Money} money.");
        }

        public void AddNewCompletedRoom()
        {
            CompletedRooms++;

            if (NeedCompletedRoomsForNextLevel == CompletedRooms)
            {
                NeedCompletedRoomsForNextLevel *= 3;

                Console.WriteLine($"Your level has been upgraded. Your new level is {++Level}. You need to complete {NeedCompletedRoomsForNextLevel - CompletedRooms} rooms for the next upgrade.");

                base.StartHealth++;
                base.Health = base.StartHealth;
                base.Armor++;
                base.Damage++;
            }
        }
    }
}
