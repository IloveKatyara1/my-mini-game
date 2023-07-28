using MiniGame.Untils;
using System;

namespace MiniGame
{
    internal class MainLogic
    {
        private readonly AskQuestion _askQuestion = new();
        private readonly Player _player = new();
        Random random = new Random();

        public void WhereGo()
        {
            Console.WriteLine($"\nwhere would you like to go?");

            char Res = _askQuestion.AskQuestionMain("L/M/R", 'L', 'M', 'R');

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
                Console.WriteLine("you found an empty room");
            }
            else if (randomNumber <= SimpleOpponent + DifficultOpponent + EmptyRoom + Something)
            {
                Console.WriteLine("you found something");
            }
        }

        public void GoNextRoom()
        {
            Console.WriteLine($"\nplease, choose the action");

            char Res = _askQuestion.AskQuestionMain($"Look statistic: S;\nlook inventory: I;\ncontinue your way: W;", 'S', 'I', 'W');

            switch (Res)
            {
                case 'W':
                    WhereGo();
                    break;
                case 'S':
                    _player.ShowStatistic();
                    break;
                case 'I':
                    Console.WriteLine("Will soon");
                    break;
            }

            GoNextRoom();
        }

        public void CreateNewEnemy(string difficulty)
        {
            Console.WriteLine($"you found a {difficulty} opponent");

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
            if(_player.Health <= 0 || enemy.Health <= 0)
            {
                Console.WriteLine($"somebody died your hp is {_player.GetHealth()}; enemy hp is {enemy.GetHealth()}");
                return;
            }

            Console.WriteLine($"what will you do? \n");

            char Res = _askQuestion.AskQuestionMain($"Look your statistic: S;\nlook inventory: I;\nlook opponent's static: O;\nattack him: A", 'S', 'I', 'O', 'A');

            switch (Res)
            {
                case 'S':
                    _player.ShowStatistic();
                    break;
                case 'I':
                    Console.WriteLine("Will soon");
                    break;
                case 'O':
                    enemy.ShowStatistic();
                    break;
                case 'A':
                    int damage = _player.AttackSomebody();

                    enemy.TakeDamage(damage);

                    Console.WriteLine($"you attacked him for {damage}hp, him hp is {enemy.GetHealth()}\n");

                    damage = enemy.AttackSomebody();
                    _player.TakeDamage(damage);

                    Console.WriteLine($"he attacked you for {damage}hp, him hp is {_player.GetHealth()}\n");
                    break;
            }

            FightingWhitEnemy(enemy);
        }
    }
}