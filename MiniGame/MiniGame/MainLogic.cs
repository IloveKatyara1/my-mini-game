using MiniGame.Untils;
using System;

namespace MiniGame
{
    internal class MainLogic
    {
        private readonly AskQuestion _askQuestion = new();
        private readonly Player _player = new();
        private readonly Inventory _inventory;
        Random random = new Random();

        public MainLogic()
        {
            _inventory = new(_player);
        }

        public void WhereGo()
        {
            Console.WriteLine($"\nWhere would you like to go?");

            string Res = _askQuestion.AskQuestionMain("L/M/R", "L", "M", "R");

            int randomNumber = random.Next(1, 101);

            int SimpleOpponent = 77;
            int DifficultOpponent = 10;
            int EmptyRoom = 8;
            int Something = 5;

            if (randomNumber <= SimpleOpponent || randomNumber <= SimpleOpponent + DifficultOpponent && _player.Lvl < 4)
            {
                CreateNewEnemy("simple");
            }
            else if (randomNumber <= SimpleOpponent + DifficultOpponent)
            {
                CreateNewEnemy("difficult");
            }
            else if (randomNumber <= SimpleOpponent + DifficultOpponent + EmptyRoom)
            {
                Console.WriteLine("You found an empty room");
            }
            else if (randomNumber <= SimpleOpponent + DifficultOpponent + EmptyRoom + Something)
            {
                Console.WriteLine("You found something");
            }

            _player.AddNewCompletedRoom();
        }

        public void GoNextRoom()
        {
            Console.WriteLine($"\nPlease, choose the action");

            string Res = _askQuestion.AskQuestionMain($"Look statistic: S;\nLook inventory: I;\nContinue your way: W;", "S", "I", "W");

            switch (Res)
            {
                case "W":
                    WhereGo();
                    break;
                case "S":
                    _player.ShowStatistic();
                    break;
                case "I":
                    _inventory.ShowInventory();
                    break;
            }

            GoNextRoom();
        }

        public void CreateNewEnemy(string difficulty)
        {
            Console.WriteLine($"You found a {difficulty} opponent");

            int Health = random.Next(85, 106); 
            int Damage = random.Next(7, 13);
            int Armor = random.Next(0, 4);

            if (difficulty == "difficult")
            {
                Health += 5;
                Damage += 2;
                Armor += 2;
            }

            if(_player.Lvl >= 5)
            {
                int PlusNum = _player.Lvl - 4;

                Health += PlusNum;
                Damage += PlusNum;
                Armor += PlusNum;
            }

            Enemy NewEnemy = new(difficulty, Health, Damage, Armor);
            NewEnemy.ShowStatistic();

            FightingWhitEnemy(NewEnemy);

            if(random.Next(1, 6) == 1)
            {
                Console.WriteLine("You found a chest, do you want to open it?");

                if (_askQuestion.AskQuestionMain($"Y/N", "Y", "N") == "N")
                    return;

                GetRandomClothes();
            }
        }

        public void GetRandomClothes(int booster = 0) 
        {
            BodyPart RandomBodyPart = (BodyPart)random.Next(0, 5);
            int Units = random.Next(1, 4) + booster;
            string Name = RandomBodyPart.ToString();
            string Type = RandomBodyPart != BodyPart.Weapon ? "armor" : "damage";

            Console.WriteLine($"You found {Name} has {Units} {Type} for {RandomBodyPart}");
            
            string Res = _askQuestion.AskQuestionMain("What do you want to do with the item?\nEquip the item: P;\nDon't take the item: D;\nPut the item in inventory: I;", "P", "D", "I");

            switch(Res)
            {
                case "D":
                    return;
                case "I":
                    _inventory.AddNewItem(Name, Type, Units, RandomBodyPart);
                    Console.WriteLine("Item added to inventory");
                    break;
                case "P":
                    _inventory.AddNewItem(Name, Type, Units, RandomBodyPart);

                    var arr = RandomBodyPart != BodyPart.Weapon ? _inventory.Armor : _inventory.WeaponInventory;

                    _inventory.EquipNewItem(arr.ElementAt(arr.Count - 1), arr.Count - 1);
                    break;
            }
        }

        public void FightingWhitEnemy(Enemy enemy)
        {
            Console.WriteLine($"What will you do? \n");

            string Res = _askQuestion.AskQuestionMain("Look your statistic: S;\nLook inventory: I;\nLook opponent's static: O;\nAttack him: A", "S", "I", "O", "A");

            switch (Res)
            {
                case "S":
                    _player.ShowStatistic();
                    break;
                case "I":
                    _inventory.ShowInventory();
                    break;
                case "O":
                    enemy.ShowStatistic();
                    break;
                case "A":
                    int Damage = _player.AttackSomebody();
                    int ShowHit = Damage - enemy.Armor < 0 ? 0 : Damage - enemy.Armor;

                    enemy.TakeDamage(Damage);

                    Console.WriteLine($"You attacked him for {ShowHit}hp, him hp is {enemy.GetHealth()}\n");

                    if (enemy.Health <= 0)
                    {
                        Console.WriteLine($"You killed the opponent your hp is {_player.GetHealth()}");
                        return;
                    }

                    Damage = enemy.AttackSomebody();
                    ShowHit = Damage - _player.Armor < 0 ? 0 : Damage - enemy.Armor;

                    _player.TakeDamage(Damage);

                    Console.WriteLine($"He attacked you for {ShowHit}hp, your hp is {_player.GetHealth()}\n");

                    if (_player.Health <= 0)
                    {
                        _player.ShowStatistic();
                        Console.WriteLine("You died");
                        Environment.Exit(0);
                    }
                    break;
            }

            FightingWhitEnemy(enemy);
        }
    }
}