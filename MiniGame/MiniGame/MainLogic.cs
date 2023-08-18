using MiniGame.Untils;
using System;

namespace MiniGame
{
    internal class MainLogic
    {
        private readonly NamesItem _namesItem = new();

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

            //if (randomNumber <= SimpleOpponent || randomNumber <= SimpleOpponent + DifficultOpponent && _player.Lvl < 4)
            //{
            //    CreateNewEnemy("simple");
            //}
            //else if (randomNumber <= SimpleOpponent + DifficultOpponent)
            //{
            //    CreateNewEnemy("difficult");
            //}
            //else if (randomNumber <= SimpleOpponent + DifficultOpponent + EmptyRoom)
            //{
            //    Console.WriteLine("You found an empty room");
            //}
            //else if (randomNumber <= SimpleOpponent + DifficultOpponent + EmptyRoom + Something)
            //{
            //    Console.WriteLine("You found something");
            //}

            Seller seller = new(_player, _inventory);

            seller.Start();

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
            int EnemyMoneyDropped = random.Next(1, 20);

            if (difficulty == "difficult")
            {
                Health += 5;
                Damage += 2;
                Armor += 2;
                EnemyMoneyDropped += 10;
            }

            if(_player.Lvl >= 5)
            {
                int PlusNum = _player.Lvl - 4;

                Health += PlusNum;
                Damage += PlusNum;
                Armor += PlusNum;
                EnemyMoneyDropped += PlusNum;
            }

            Enemy NewEnemy = new(difficulty, Health, Damage, Armor);
            NewEnemy.ShowStatistic();

            FightingWhitEnemy(NewEnemy);

            Console.WriteLine($"After killed enemy, he droped {EnemyMoneyDropped} money");
            _player.ModifyMoney(EnemyMoneyDropped);

            if (random.Next(1, 9) == 1)
            {
                Console.WriteLine("You found a chest, do you want to open it?");

                if (_askQuestion.AskQuestionMain($"Y/N", "Y", "N") == "N")
                    return;

                int Units;

                int RandomNumberForUnits = random.Next(0, 9);

                if (RandomNumberForUnits <= 5)
                    Units = 1;
                else if (RandomNumberForUnits <= 7)
                    Units = 2;
                else
                    Units = 3;

                BodyPart RandomBodyPart = (BodyPart)random.Next(0, 5);
                string Name = _namesItem.GetName(RandomBodyPart.ToString(), Units - 1);
                string Type = RandomBodyPart != BodyPart.Weapon ? "armor" : "damage";

                ChooseActionForFoundItem(Name, Type, Units, RandomBodyPart, null);

                if (random.Next(1, 5) == 1)
                {
                    if (RandomNumberForUnits <= 5)
                        EnemyMoneyDropped = 10;
                    else if (RandomNumberForUnits <= 7)
                        EnemyMoneyDropped = 20;
                    else
                        EnemyMoneyDropped = 30;

                    Console.WriteLine($"You found {EnemyMoneyDropped} money");
                }


                if (random.Next(1, 6) == 1)
                {
                    int NameIndex;

                    RandomNumberForUnits = random.Next(0, 9);

                    if (RandomNumberForUnits <= 5)
                    {
                        Units = 20;
                        NameIndex = 0;
                    }
                    else if (RandomNumberForUnits <= 7)
                    {
                        Units = 50;
                        NameIndex = 1;
                    }
                    else
                    {
                        Units = 100;
                        NameIndex = 2;
                    }

                    Name = _namesItem.GetName("Heal", NameIndex);
                    Type = "heal";

                    ChooseActionForFoundItem(Name, Type, Units, null, NameIndex + 1);
                }
            }
        }

        public void ChooseActionForFoundItem(string name, string type, int units, BodyPart? bodyPart, int? unitsForSale)
        {
            Console.WriteLine($"You found {name} has {units} {type}{(bodyPart.HasValue ? $" for {bodyPart}" : "")}");

            string Res = _askQuestion.AskQuestionMain($"What do you want to do with the item?\n{(bodyPart.HasValue ? "Equip" : "Use")} the item: P;\nDon't take the item: D;\nPut the item in inventory: I;", "P", "D", "I");

            switch (Res)
            {
                case "D":
                    return;
                case "I":
                    if (bodyPart.HasValue)
                        _inventory.AddNewItem(name, type, units, (BodyPart)bodyPart);
                    else if (unitsForSale.HasValue)
                        _inventory.AddNewItem(name, type, units, (int)unitsForSale);
                    else
                        throw new ArgumentException("didn't have argument bodyPart or unitsForSale");

                    Console.WriteLine("Item added to inventory");
                    break;
                case "P":
                    if (bodyPart.HasValue)
                    {
                        _inventory.AddNewItem(name, type, units, (BodyPart)bodyPart);

                        List<Dictionary<string, string>> arr;

                        arr = bodyPart != BodyPart.Weapon ? _inventory.Armor : _inventory.WeaponInventory;

                        _inventory.EquipClothest(arr.ElementAt(arr.Count - 1), arr.Count - 1);
                    }
                    else if (unitsForSale.HasValue)
                    {
                        _inventory.AddNewItem(name, type, units, (int)unitsForSale);
                        _inventory.UseItem(_inventory.Other[_inventory.Other.Count - 1]);
                    }
                    else
                        throw new ArgumentException("didn't have argument bodyPart or unitsForSale");
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