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

            int SimpleOpponent = 70;
            int DifficultOpponent = 15;
            int EmptyRoom = 10;
            int Something = 5;

            Console.WriteLine(randomNumber);

            if (randomNumber <= SimpleOpponent)
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

            int Health = random.Next(80, 121); 
            int Damage = random.Next(5, 15);

            if (difficulty == "difficult")
            {
                Health += 5;
                Damage += 2;
            }

            Enemy NewEnemy = new(difficulty, Health, Damage);
            NewEnemy.ShowStatistic();

            FightingWhitEnemy(NewEnemy);
        }

        public void FightingWhitEnemy(Enemy enemy)
        {
            if(_player.Health <= 0)
            {
                _player.ShowStatistic();
                Console.WriteLine("You died");
                Environment.Exit(0);
                return;
            } else if(enemy.Health <= 0)
            {
                Console.WriteLine($"You killed the enemy your hp is {_player.GetHealth()}");
                return;
            }

            Console.WriteLine($"What will you do? \n");

            string Res = _askQuestion.AskQuestionMain($"Look your statistic: S;\nLook inventory: I;\nLook opponent's static: O;\nAttack him: A", "S", "I", "O", "A");

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

                    Damage = enemy.AttackSomebody();
                    ShowHit = Damage - _player.Armor < 0 ? 0 : Damage - enemy.Armor;

                    _player.TakeDamage(Damage);

                    Console.WriteLine($"He attacked you for {ShowHit}hp, your hp is {_player.GetHealth()}\n");
                    break;
            }

            FightingWhitEnemy(enemy);
        }
    }
}