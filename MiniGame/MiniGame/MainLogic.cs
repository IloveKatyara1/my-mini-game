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

            string Res = _askQuestion.AskQuestionMain($"Look statistic: S;\nlook inventory: I;\ncontinue your way: W;", "S", "I", "W");

            switch (Res)
            {
                case "W":
                    WhereGo();
                    break;
                case "S":
                    _player.ShowStatistic();
                    break;
                case "I":
                    LookInventory();
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

            string Res = _askQuestion.AskQuestionMain($"Look your statistic: S;\nlook inventory: I;\nlook opponent's static: O;\nattack him: A", "S", "I", "O", "A");

            switch (Res)
            {
                case "S":
                    _player.ShowStatistic();
                    break;
                case "I":
                    LookInventory();
                    break;
                case "O":
                    enemy.ShowStatistic();
                    break;
                case "A":
                    int Damage = _player.AttackSomebody();

                    enemy.TakeDamage(Damage);

                    Console.WriteLine($"you attacked him for {Damage}hp, him hp is {enemy.GetHealth()}\n");

                    Damage = enemy.AttackSomebody();
                    _player.TakeDamage(Damage);

                    Console.WriteLine($"he attacked you for {Damage}hp, him hp is {_player.GetHealth()}\n");
                    break;
            }

            FightingWhitEnemy(enemy);
        }

        public void LookInventory()
        {
            int lenght = _player.ShowInventory();

            if (lenght == 0)
                return;

            List<string> ArrayStr = new List<string>();

            for(int i = 1; i < lenght; i++)
            {
                ArrayStr.Add(i.ToString());
            }

            ArrayStr.Add("E");

            string Res = _askQuestion.AskQuestionMain($"select item by type number, or exit: E", ArrayStr.ToArray());

            if (Res == "E")
                return;

            var item = _player.GetItemByNum(int.Parse(Res));

            string Name = item.Keys.FirstOrDefault();
            int Armor = item[Name]["armor"];

            Console.WriteLine($"You selected: {Name}, Armor: {Armor}");

            string ResSecond = _askQuestion.AskQuestionMain($"what do you want to do whith this item \nPut on the item: P; \nThrow away the item: T;\nBack to all items: B;\nExit: E", "P", "T", "B", "E");

            switch(ResSecond) 
            {
                case "P":
                    Console.WriteLine("will soon");
                    break;
                case "T":
                    Console.WriteLine("will soon");
                    break;
                case "B":
                    LookInventory();
                    break;
                case "E":
                    return;
            }
        }
    }
}