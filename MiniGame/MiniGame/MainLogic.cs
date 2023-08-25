using MiniGame.Enums;
using MiniGame.Untils;
using System;

namespace MiniGame
{
    internal class MainLogic
    {
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

            string Res = AskQuestion.Main("L/M/R", "L", "M", "R");

            int randomNumber = random.Next(1, 101);

            int SimpleOpponent = 77;
            int DifficultOpponent = 10;
            int EmptyRoom = 8;
            int Something = 5;

            if (_player.CompletedRooms != 0 && _player.CompletedRooms % 9 == 0)
            {
                CreateNewEnemy(DiffucultEnemyes.Boss);
            }
            else
            {
                if (randomNumber <= SimpleOpponent || randomNumber <= SimpleOpponent + DifficultOpponent && _player.Lvl < 4)
                    CreateNewEnemy(DiffucultEnemyes.Normal);
                else if (randomNumber <= SimpleOpponent + DifficultOpponent)
                    CreateNewEnemy(DiffucultEnemyes.Diffuclt);
                else if (randomNumber <= SimpleOpponent + DifficultOpponent + EmptyRoom)
                    Console.WriteLine("You found an empty room");
                else if (randomNumber <= SimpleOpponent + DifficultOpponent + EmptyRoom + Something)
                {
                    switch (Res)
                    {
                        case "L":
                            Seller seller = new(_player, _inventory);
                            seller.Start();
                            break;
                        case "M":
                            FoundChest(maxRandomForHeal: 6, maxRanbomForMoney: 5, fromBoss: false);
                            break;
                        case "R":
                            CreateNewEnemy(DiffucultEnemyes.Diffuclt);
                            break;
                    }
                }
            }

            _player.AddNewCompletedRoom();
        }

        public void GoNextRoom()
        {
            Console.WriteLine($"\nPlease, choose the action");

            string Res = AskQuestion.Main($"Look statistic: S;\nLook inventory: I;\nContinue your way: W;", "S", "I", "W");

            switch (Res)
            {
                case "W":
                    WhereGo();
                    break;
                case "S":
                    _player.ShowStatistic();
                    break;
                case "I":
                    _inventory.Show_inventory();
                    break;
            }

            GoNextRoom();
        }

        public void CreateNewEnemy(DiffucultEnemyes difficulty)
        {
            Console.WriteLine($"You found a {difficulty.ToString().ToLower()} opponent");

            int Health = random.Next(85, 106); 
            int Damage = random.Next(7, 13);
            int Armor = random.Next(0, 4);
            int EnemyMoneyDropped = random.Next(1, 20);

            if (difficulty == DiffucultEnemyes.Diffuclt)
            {
                Health += 5;
                Damage += 2;
                Armor += 2;
                EnemyMoneyDropped += 10;
            } 
            else if (difficulty == DiffucultEnemyes.Boss)
            {
                Health += 25;
                Damage += 20;
                Armor += 8;
                EnemyMoneyDropped += 600;
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

            Console.WriteLine($"After killed {(difficulty == DiffucultEnemyes.Boss ? "boss" : "enemy")}, he droped {EnemyMoneyDropped} money");
            _player.ModifyMoney(EnemyMoneyDropped);

            if(difficulty == DiffucultEnemyes.Boss)
            {
                FoundChest(maxRandomForHeal: 2, maxRanbomForMoney: 2, fromBoss: true);
                FoundChest(maxRandomForHeal: 2, maxRanbomForMoney: 2, fromBoss: true);

                Seller Seller = new(_player, _inventory);
                Seller.Start();
            }
            else if (random.Next(1, 9) == 1)
            {
                FoundChest(maxRandomForHeal: 6, maxRanbomForMoney: 5, fromBoss: false);
            }
        }

        public void FoundChest(int maxRandomForHeal, int maxRanbomForMoney, bool fromBoss)
        {
            Console.WriteLine("You found a chest, do you want to open it?");

            if (AskQuestion.Main($"Y/N", "Y", "N") == "N")
                return;

            int Units;

            int RandomNumberForUnits = random.Next(0, 9);

            if (fromBoss)
            {
                if (RandomNumberForUnits <= 5)
                    Units = 3;
                else
                    Units = 4;

                Units += _player.Lvl >= 5 ? _player.Lvl - 5 : 0;
            }
            else
            {
                if (RandomNumberForUnits <= 5)
                    Units = 1;
                else if (RandomNumberForUnits <= 7)
                    Units = 2;
                else
                    Units = 3;
            }

            BodyPart RandomBodyPart = (BodyPart)random.Next(0, 5);
            string Name = NamesItem.GetName(RandomBodyPart.ToString(), Units - 1);
            string Type = RandomBodyPart != BodyPart.Weapon ? "armor" : "damage";

            ChooseActionForFoundItem(Name, Type, Units, RandomBodyPart, null);

            if (random.Next(1, maxRanbomForMoney) == 1)
            {
                int MoneyDropped;

                if (fromBoss)
                {
                    if (RandomNumberForUnits <= 5)
                        MoneyDropped = 50;
                    else
                        MoneyDropped = 100;

                    MoneyDropped += _player.Lvl >= 5 ? _player.Lvl - 5 : 0 * 2;
                }
                else
                {
                    if (RandomNumberForUnits <= 5)
                        MoneyDropped = 10;
                    else if (RandomNumberForUnits <= 7)
                        MoneyDropped = 20;
                    else
                        MoneyDropped = 30;

                    MoneyDropped += _player.Lvl >= 5 ? _player.Lvl - 5 : 0 * 2;
                }

                Console.WriteLine($"You found {MoneyDropped} money");
                _player.ModifyMoney(MoneyDropped);
            }


            if (random.Next(1, maxRandomForHeal) == 1)
            {
                int NameIndex;

                RandomNumberForUnits = random.Next(0, 9);

                if (fromBoss)
                {
                    if (RandomNumberForUnits <= 4)
                    {
                        Units = 100;
                        NameIndex = 2;
                    }
                    else
                    {
                        Units = 999;
                        NameIndex = 3;
                    }
                }
                else
                {
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
                }

                Name = NamesItem.GetName("Heal", NameIndex);
                Type = "heal";

                ChooseActionForFoundItem(Name, Type, Units, null, NameIndex + 1);
            }
        }

        public void ChooseActionForFoundItem(string name, string type, int units, BodyPart? bodyPart, int? unitsForSale)
        {
            Console.WriteLine($"You found {name} has {units} {type}{(bodyPart.HasValue ? $" for {bodyPart}" : "")}");

            string Res = AskQuestion.Main(
                $"What do you want to do with the item?" +
                $"\n{(bodyPart.HasValue ? "Equip" : "Use")} the item: P;" +
                $"\nDon't take the item: D;" +
                $"\nPut the item in inventory: I;", "P", "D", "I");

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

                    Console.WriteLine("Item added to _inventory");
                    break;
                case "P":
                    if (bodyPart.HasValue)
                    {
                        _inventory.AddNewItem(name, type, units, (BodyPart)bodyPart);

                        List<Dictionary<string, string>> arr;

                        arr = bodyPart != BodyPart.Weapon ? _inventory.Armor : _inventory.Weapon_inventory;

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
            Console.WriteLine($"What will you do?");

            string Res = AskQuestion.Main(
                "Look your statistic: S;" +
                "\nAuto fighting: U;" +
                "\nLook inventory: I;" +
                "\nLook opponent's static: O;" +
                "\nAttack him: A", 
                "S", "I", "O", "A", "U"
                );

            switch (Res)
            {
                case "S":
                    _player.ShowStatistic();
                    break;
                case "I":
                    _inventory.Show_inventory();
                    break;
                case "O":
                    enemy.ShowStatistic();
                    break;
                case "U":
                    while(enemy.Health != 0 || _player.Health != 0)
                    {
                        if(enemy.Damage + 3 - _player.Armor >= _player.Health && enemy.Health > _player.Damage - 3 - enemy.Armor)
                        {
                            Console.WriteLine("If you continue auto fighting, you can die");

                            if (AskQuestion.Main("Are you sure want to continue auto fighting. Y/N", "Y", "N") == "N")
                                break;
                        }
                        Attack(enemy);
                    }
                    break;
                case "A":
                    Attack(enemy);
                    break;
            }

            FightingWhitEnemy(enemy);
        }

        private void Attack(Enemy enemy)
        {
            int EntityDamage = enemy.TakeDamage(_player.AttackSomebody());
            Console.WriteLine($"You attacked him for {EntityDamage}hp, him hp is {enemy.GetHealth()}\n");

            if (enemy.Health == 0)
            {
                Console.WriteLine($"You killed the opponent your hp is {_player.GetHealth()}");
                return;
            }

            EntityDamage = _player.TakeDamage(enemy.AttackSomebody());
            Console.WriteLine($"He attacked you for {EntityDamage}hp, your hp is {_player.GetHealth()}\n");

            if (_player.Health == 0)
            {
                _player.ShowStatistic();
                Console.WriteLine("You died");
                Environment.Exit(0);
            }
        }
    }
}