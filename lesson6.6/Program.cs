using System;
using System.Collections.Generic;

namespace Fight
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isPlaying = true;
            Console.CursorVisible = false;

            while (isPlaying)
            {
                int serialNumber = 1;
                List<Ninja> ninjas = new List<Ninja>();
                ninjas.Add(new FireNinja(serialNumber++, "Горенье", 450, 100));
                ninjas.Add(new StoneNinja(serialNumber++, "Скала", 500, 50));
                ninjas.Add(new WaterNinja(serialNumber++, "Водяной", 470, 80));
                ninjas.Add(new AirNinja(serialNumber++, "Ветерок", 475, 75));
                ninjas.Add(new ElectricNinja(serialNumber++, "Молния", 465, 85));
                Battlefield battlefield = new Battlefield(ninjas);
                Console.WriteLine("Для выхода нажмите ESC\nДля продолжния - любую другую клавишу");
                ConsoleKeyInfo key = Console.ReadKey();
                Console.Clear();

                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        isPlaying = false;
                        break;
                    default:
                        battlefield.StartFight();
                        break;
                }
                Console.Clear();
            }
        }
    }

    abstract class Ninja
    {
        protected string SkillName;
        protected int Damage;
        protected int CriticalStrikeChance;
        protected int MissChance;
        protected int SkillDamage;
        private int _health;
        private int _maxHealth;
        private int _skillChance = 3;
        private static Random _random = new Random();

        public int SerialNumber { get; protected set; }

        public string Name { get; protected set; }

        public int Health 
        {
            get 
            {
                if (_health < 0)
                {
                    return 0;
                }
                else
                {
                    return _health;
                }
            }
            protected set
            {
                _health = value;
            }
        }

        public bool IsAlive 
        { 
            get 
            {
                return _health > 0;
            } 
        }

        public Ninja(int serialNumber, string name, int health, int damage)
        {
            _health = health;
            SerialNumber = serialNumber;
            Name = name;
            Damage = damage;
            _maxHealth = health;
        }

        public virtual int UseSkill()
        {
            int skillDemage = SkillDamage;
            PrintColorMessage($"{Name} использовал специальное умение - {SkillName}", ConsoleColor.Green);
            return skillDemage;
        }

        public int CauseDamage()
        {
            int definitiveDamage = Damage;

            if (Health > 0)
            {
                if (CheckChance(MissChance))
                {
                    definitiveDamage = 0;
                    PrintColorMessage($"{Name} промахнулся!", ConsoleColor.Yellow);
                }
                else if (CheckChance(CriticalStrikeChance))
                {
                    definitiveDamage *= 2;
                    PrintColorMessage($"{Name} нанес критический урон Х2", ConsoleColor.Yellow);
                }
                else if (CheckChance(_skillChance))
                {
                    definitiveDamage = UseSkill();
                }
                else
                {
                    Console.WriteLine($"{Name} нанес удар.");
                }
            }
            else
            {
                definitiveDamage = 0;
            }

            return definitiveDamage;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }

        public void DrawBar(ConsoleColor color)
        {
            int sections = 10;
            double fullBarPercent = 100.0;
            double percentPerStep = fullBarPercent / sections;
            double onePercent = _maxHealth / fullBarPercent;
            double completion = Health / onePercent;
            int paintCell = (int)Math.Round(completion / percentPerStep, MidpointRounding.AwayFromZero);
            int emptycell = sections - paintCell;
            ConsoleColor defaultColor = Console.BackgroundColor;
            Console.Write("[");
            Console.BackgroundColor = color;

            for (int i = 0; i < paintCell; i++)
            {
                Console.Write("_");
            }

            Console.BackgroundColor = defaultColor;

            for (int i = 0; i < emptycell; i++)
            {
                Console.Write("_");
            }

            Console.Write("]");
            Console.WriteLine($"\t{Name} - {Health}");
        }

        public void ShowInfo()
        {
            Console.WriteLine($"{SerialNumber}. {Name}");
        }

        private bool CheckChance(int chance)
        {
            int randomNumber = _random.Next(1, 101);
            return chance >= randomNumber;
        }

        private void PrintColorMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    class FireNinja : Ninja
    {
        public FireNinja(int serialNumber, string name, int health, int damage) : base(serialNumber, name, health, damage)
        {
            SkillDamage = 300;
            CriticalStrikeChance = 7;
            MissChance = 9;
        }

        public override int UseSkill()
        {
            SkillName = "Извержение вулкана";
            base.UseSkill();
            return SkillDamage;
        }
    }

    class StoneNinja : Ninja
    {
        public StoneNinja(int serialNumber, string name, int health, int damage) : base(serialNumber, name, health, damage)
        {
            SkillDamage = 0;
            CriticalStrikeChance = 10;
            MissChance = 12;
        }

        public override int UseSkill()
        {
            int newMissChance = 0;
            SkillName = "Камень удачи";
            base.UseSkill();
            MissChance = newMissChance;
            return SkillDamage;

        }
    }

    class WaterNinja : Ninja
    {
        public WaterNinja(int serialNumber, string name, int health, int damage) : base(serialNumber, name, health, damage)
        {
            SkillDamage = 275;
            CriticalStrikeChance = 8;
            MissChance = 10;
        }

        public override int UseSkill()
        {
            SkillName = "Гейзер";
            base.UseSkill();
            return SkillDamage;
        }
    }

    class AirNinja : Ninja
    {
        public AirNinja(int serialNumber, string name, int health, int damage) : base(serialNumber, name, health, damage)
        {
            SkillDamage = 0;
            CriticalStrikeChance = 9;
            MissChance = 11;
        }

        public override int UseSkill()
        {
            int specialSkillPower = 200;
            SkillName = "Гоный воздух";
            base.UseSkill();
            Health += specialSkillPower;
            return SkillDamage;
        }
    }

    class ElectricNinja : Ninja
    {
        public ElectricNinja(int serialNumber, string name, int health, int damage) : base(serialNumber, name, health, damage)
        {
            SkillDamage = 0;
            CriticalStrikeChance = 9;
            MissChance = 9;
        }

        public override int UseSkill()
        {
            int specialSkillPower = 40;
            SkillName = "Заряженная атмосфера";
            base.UseSkill();
            CriticalStrikeChance += specialSkillPower;
            return SkillDamage;
        }
    }

    class Battlefield
    {
        private List<Ninja> _ninjas;

        public Battlefield(List<Ninja> ninjas)
        {
            _ninjas = ninjas;
        }

        public void StartFight()
        {
            bool isFind = false;
            Console.WriteLine("Перед Вами ниндзя из враждующих кланов.\n");
            ShowAllNinjas();
            Console.WriteLine();
            Console.Write("Выберите первого ниндзя: ");
            isFind = TryChooseNinja(out Ninja firstNinja);

            if (isFind)
            {
                Console.Write("Выберите второго ниндзя: ");
                isFind = TryChooseNinja(out Ninja secondNinja);

                if (isFind && firstNinja.SerialNumber != secondNinja.SerialNumber)
                {
                    Console.WriteLine();
                    firstNinja.DrawBar(ConsoleColor.Red);
                    secondNinja.DrawBar(ConsoleColor.Blue);
                    Console.WriteLine("\nХаджимэ!\n");

                    while (firstNinja.IsAlive && secondNinja.IsAlive)
                    {
                        secondNinja.TakeDamage(firstNinja.CauseDamage());
                        firstNinja.TakeDamage(secondNinja.CauseDamage());
                        firstNinja.DrawBar(ConsoleColor.Red);
                        secondNinja.DrawBar(ConsoleColor.Blue);
                        Console.WriteLine("***\n");
                        Console.ReadKey();
                    }

                    DeclareWinner(firstNinja, secondNinja);
                    Console.ReadKey();
                }
                else
                {
                    ErrorMasseage();
                }
            }
            else
            {
                ErrorMasseage();
            }
        }

        public void ShowAllNinjas()
        {
            foreach (Ninja ninja in _ninjas)
            {
                ninja.ShowInfo();
            }
        }

        private bool TryChooseNinja(out Ninja ninja)
        {
            bool success;
            bool isFind;
            string userInput = Console.ReadLine();
            success = int.TryParse(userInput, out int ninjaSerialNumber);
            isFind = TryFindNinja(ninjaSerialNumber, out ninja);
            return success && isFind;
        }
        private bool TryFindNinja(int serialNumber, out Ninja fighter)
        {
            bool isFind = false;
            fighter = null;

            foreach (Ninja ninja in _ninjas)
            {
                if (ninja.SerialNumber == serialNumber)
                {
                    isFind = true;
                    fighter = ninja;
                    break;
                }
            }
            return isFind;
        }

        private void DeclareWinner(Ninja firstNinja, Ninja secondNinja)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            if (firstNinja.Health == 0)
            {
                Console.WriteLine($"\nПобедил {secondNinja.Name}");
            }
            else
            {
                Console.WriteLine($"\nПобедил {firstNinja.Name}");
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        private void ErrorMasseage()
        {
            Console.Clear();
            Console.Write("Некорректный выбор, попробуйте еще раз.");
            Console.ReadKey();
        }
    }
}
