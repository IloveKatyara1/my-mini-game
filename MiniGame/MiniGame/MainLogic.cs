using MiniGame.Enums;
using MiniGame.Models;
using MiniGame.Untils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniGame
{
    internal class MainLogic
    {
        private readonly Player _player;
        private readonly Inventory _inventory;
        Random random = new Random();

        private bool _madeRecentlySave = false;

        public MainLogic()
        {
            if (!File.Exists("save.json"))
                File.Create("save.json").Close();

            string JsonData = File.ReadAllText("save.json");

            Console.WriteLine("\nPlease, choose an action");
            string Response = AskQuestion.Main("Load the save: L;\nStart new game: N;\nExit from game: E;", "L", "N", "E");

            switch (Response)
            {
                case "E":
                    return;
                case "L":
                    if (string.IsNullOrEmpty(JsonData))
                    {
                        Response = AskQuestion.Main("You haven't save. Would you like to start new game? Y/N", "Y", "N");
                        if (Response == "N")
                            return;

                        Console.WriteLine("When you got up, you saw that you are in dungeon, and you must go out from here!");

                        _player = new();
                        _inventory = new(_player);

                        break;
                    }

                    Save SaveData = JsonConvert.DeserializeObject<Save>(JsonData);

                    _player = new Player(SaveData.Level, SaveData.Health, SaveData.StartHealth, 
                        SaveData.Armor, SaveData.Damage, SaveData.CompletedRooms, 
                        SaveData.NeedCompletedRoomsForNextLevel, SaveData.Money);
                    _inventory = new(_player, SaveData.ArmorInventory, SaveData.WeaponInventory, SaveData.Other, SaveData.Equipped);
                    break;
                case "N":
                    Console.WriteLine("When you got up, you saw that you are in dungeon, and you must go out from here!");

                    _player = new();
                    _inventory = new(_player);
                    break;
            }

            GoNextRoom();
        }

        public void Menu()
        {
            Console.WriteLine("\nPlease, choose an action");

            string Response = AskQuestion.Main("Exit from game: E;\nContinue game: C;\nSave game: S;", "E", "C", "S");

            switch(Response)
            {
                case "E":
                    if(_madeRecentlySave == false)
                    {
                        Console.WriteLine("If you will continue, you will lose your progres. Are you sure want continue?");

                        Response = AskQuestion.Main("Continue: C;\nBack: B;\nMake Save and exit: S", "C", "B", "S");

                        if (Response == "B")
                            break;
                        else if(Response == "S")
                            MakeSave();
                    }
                    Environment.Exit(0);
                    break;
                case "C":
                    _madeRecentlySave = false;
                    return;
                case "S":
                    MakeSave();
                    _madeRecentlySave = true;
                    break;
            }

            Menu();
        }

        public void WhereGo()
        {
            Console.WriteLine("\nWhere would you like to go?");

            string Response = AskQuestion.Main("L/M/R", "L", "M", "R");

            int RandomNumber = random.Next(1, 101);

            int SimpleOpponent = 77;
            int DifficultOpponent = 10;
            int EmptyRoom = 8;
            int Something = 5;

            if (_player.CompletedRooms != 0 && _player.CompletedRooms % 9 == 0)
            {
                CreateNewEnemy(DifficultEnemies.Boss);
            }
            else
            {
                if (RandomNumber <= SimpleOpponent || (RandomNumber <= SimpleOpponent + DifficultOpponent && _player.Level < 4))
                    CreateNewEnemy(DifficultEnemies.Normal);
                else if (RandomNumber <= SimpleOpponent + DifficultOpponent)
                    CreateNewEnemy(DifficultEnemies.Difficult);
                else if (RandomNumber <= SimpleOpponent + DifficultOpponent + EmptyRoom)
                    Console.WriteLine("You found an empty room");
                else if (RandomNumber <= SimpleOpponent + DifficultOpponent + EmptyRoom + Something)
                {
                    switch (Response)
                    {
                        case "L":
                            Seller seller = new(_player, _inventory);
                            seller.Start();
                            break;
                        case "M":
                            FoundChest(maxRandomForHeal: 6, maxRandomForMoney: 5, fromBoss: false);
                            break;
                        case "R":
                            CreateNewEnemy(DifficultEnemies.Difficult);
                            break;
                    }
                }
            }

            _player.AddNewCompletedRoom();
        }

        public void GoNextRoom()
        {
            Console.WriteLine("\nPlease, choose an action");

            string Response = AskQuestion.Main("Look at statistics: S;\nLook at inventory: I;\nContinue your way: W;\nGo to menu: E", "S", "I", "W", "E");

            switch (Response)
            {
                case "W":
                    WhereGo();
                    break;
                case "S":
                    _player.ShowStatistics();
                    break;
                case "I":
                    _inventory.ShowInventory();
                    break;
                case "E":
                    Menu();
                    break;
            }

            GoNextRoom();
        }

        public void CreateNewEnemy(DifficultEnemies difficulty)
        {
            Console.WriteLine($"You found a {difficulty.ToString().ToLower()} opponent");

            int Health = random.Next(85, 101);
            int Damage = random.Next(7, 11);
            int Armor = random.Next(0, 4);
            int EnemyMoneyDropped = random.Next(1, 20);

            if (difficulty == DifficultEnemies.Difficult)
            {
                Health += 5;
                Damage += 2;
                Armor += 2;
                EnemyMoneyDropped += 10;
            }
            else if (difficulty == DifficultEnemies.Boss)
            {
                Health += 25;
                Damage += 10;
                Armor += 8;
                EnemyMoneyDropped += 600;
            }

            if (_player.Level >= 5)
            {
                int plusNum = _player.Level - 4;

                Health += plusNum;
                Damage += plusNum;
                Armor += plusNum;
                EnemyMoneyDropped += plusNum;
            }

            Enemy NewEnemy = new(difficulty, Health, Damage, Armor);
            NewEnemy.ShowStatistics();

            FightingWithEnemy(NewEnemy);

            Console.WriteLine($"After killing the {(difficulty == DifficultEnemies.Boss ? "boss" : "enemy")}, they dropped {EnemyMoneyDropped} money");
            _player.ModifyMoney(EnemyMoneyDropped);

            if (difficulty == DifficultEnemies.Boss)
            {
                FoundChest(maxRandomForHeal: 2, maxRandomForMoney: 2, fromBoss: true);
                FoundChest(maxRandomForHeal: 2, maxRandomForMoney: 2, fromBoss: true);

                Seller seller = new(_player, _inventory);
                seller.Start();
            }
            else if (random.Next(1, 9) == 1)
            {
                FoundChest(maxRandomForHeal: 6, maxRandomForMoney: 5, fromBoss: false);
            }
        }

        public void FoundChest(int maxRandomForHeal, int maxRandomForMoney, bool fromBoss)
        {
            Console.WriteLine("You found a chest, do you want to open it?");

            if (AskQuestion.Main("Y/N", "Y", "N") == "N")
                return;

            int Units;

            int RandomNumberForUnits = random.Next(0, 9);

            if (fromBoss)
            {
                if (RandomNumberForUnits <= 5)
                    Units = 3;
                else
                    Units = 4;

                Units += _player.Level >= 5 ? _player.Level - 5 : 0;
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

            if (random.Next(1, maxRandomForMoney) == 1)
            {
                int MoneyDropped;

                if (fromBoss)
                {
                    if (RandomNumberForUnits <= 5)
                        MoneyDropped = 50;
                    else
                        MoneyDropped = 100;

                    MoneyDropped += _player.Level >= 5 ? (_player.Level - 5) * 2 : 0;
                }
                else
                {
                    if (RandomNumberForUnits <= 5)
                        MoneyDropped = 10;
                    else if (RandomNumberForUnits <= 7)
                        MoneyDropped = 20;
                    else
                        MoneyDropped = 30;

                    MoneyDropped += _player.Level >= 5 ? (_player.Level - 5) * 2 : 0;
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
            Console.WriteLine($"You found {name} with {units} {type}{(bodyPart.HasValue ? $" for {bodyPart}" : "")}");

            string response = AskQuestion.Main(
                $"What do you want to do with the item?" +
                $"\n{(bodyPart.HasValue ? "Equip" : "Use")} the item: P;" +
                $"\nDon't take the item: D;" +
                $"\nPut the item in inventory: I;", "P", "D", "I");

            switch (response)
            {
                case "D":
                    return;
                case "I":
                    if (bodyPart.HasValue)
                        _inventory.AddNewItem(name, type, units, (BodyPart)bodyPart);
                    else if (unitsForSale.HasValue)
                        _inventory.AddNewItem(name, type, units, (int)unitsForSale);
                    else
                        throw new ArgumentException("Argument bodyPart or unitsForSale is missing");

                    Console.WriteLine("Item added to inventory");
                    break;
                case "P":
                    if (bodyPart.HasValue)
                    {
                        _inventory.AddNewItem(name, type, units, (BodyPart)bodyPart);

                        List<Dictionary<string, string>> arr;

                        arr = bodyPart != BodyPart.Weapon ? _inventory.Armor : _inventory.WeaponInventory;

                        _inventory.EquipClothes(arr.ElementAt(arr.Count - 1), arr.Count - 1);
                    }
                    else if (unitsForSale.HasValue)
                    {
                        _inventory.AddNewItem(name, type, units, (int)unitsForSale);
                        _inventory.UseItem(_inventory.Other[_inventory.Other.Count - 1]);
                    }
                    else
                        throw new ArgumentException("Argument bodyPart or unitsForSale is missing");
                    break;
            }
        }

        public void FightingWithEnemy(Enemy enemy)
        {
            if (enemy.Health == 0)
                return;

            Console.WriteLine("What will you do?");

            string Response = AskQuestion.Main(
                "Look at your statistics: S;" +
                "\nAuto fight: U;" +
                "\nLook at inventory: I;" +
                "\nLook at opponent's statistics: O;" +
                "\nAttack: A",
                "S", "I", "O", "A", "U"
                );

            switch (Response)
            {
                case "S":
                    _player.ShowStatistics();
                    break;
                case "I":
                    _inventory.ShowInventory();
                    break;
                case "O":
                    enemy.ShowStatistics();
                    break;
                case "U":
                    while (enemy.Health != 0 && _player.Health != 0)
                    {
                        if (enemy.Damage + 3 - _player.Armor >= _player.Health && enemy.Health > _player.Damage - 3 - enemy.Armor)
                        {
                            Console.WriteLine("If you continue auto fighting, you might die");
                            if (AskQuestion.Main("Are you sure you want to continue auto fighting? Y/N", "Y", "N") == "N")
                                break;
                        }
                        Attack(enemy);
                    }
                    break;
                case "A":
                    Attack(enemy);
                    break;
            }

            FightingWithEnemy(enemy);
        }

        private void Attack(Enemy enemy)
        {
            int EntityDamage = enemy.TakeDamage(_player.AttackSomebody());
            Console.WriteLine($"You attacked them for {EntityDamage}hp; their hp is {enemy.GetHealth()}\n");

            if (enemy.Health == 0)
            {
                Console.WriteLine($"You killed the opponent; your hp is {_player.GetHealth()}");
                return;
            }

            EntityDamage = _player.TakeDamage(enemy.AttackSomebody());
            Console.WriteLine($"He attacked you for {EntityDamage}hp; your hp is {_player.GetHealth()}\n");

            if (_player.Health == 0)
            {
                _player.ShowStatistics();
                Console.WriteLine("You died");
                Environment.Exit(0);
            }
        }

        public void MakeSave()
        {
            File.WriteAllText("save.json", string.Empty);

            var saveData = new Save
            {
                Level = _player.Level,
                Health = _player.Health,
                StartHealth = _player.StartHealth,
                Armor = _player.Armor,
                Damage = _player.Damage,
                CompletedRooms = _player.CompletedRooms,
                NeedCompletedRoomsForNextLevel = _player.NeedCompletedRoomsForNextLevel,
                Money = _player.Money,
                ArmorInventory = _inventory.Armor,
                WeaponInventory = _inventory.WeaponInventory,
                Other = _inventory.Other,
                Equipped = _inventory.Equipped
            };

            string jsonData = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            File.WriteAllText("save.json", jsonData);

            Console.WriteLine("game saved");
        }
    }
}
