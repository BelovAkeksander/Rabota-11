using System;
using System.Collections.Generic;

public class Bank
{
    public string Name { get; set; }
    public List<Branch> Branches { get; set; } = new List<Branch>();
    public List<Deposit> Deposits { get; set; } = new List<Deposit>();  // Список всех вкладов

    public Bank(string name)
    {
        Name = name;
    }

    public void DisplayBankInfo()
    {
        Console.WriteLine($"Банк: {Name}");
        Console.WriteLine($"Количество филиалов: {Branches.Count}");
        foreach (var branch in Branches)
        {
            Console.WriteLine($"{branch.Name}: Общая сумма вкладов: {branch.TotalDeposits:C}");
        }
        Console.WriteLine($"Общее количество вкладов: {Deposits.Count}");  // Отображение количества всех вкладов
    }
}


public class Branch
{
    public string Name { get; set; }
    public List<Deposit> Deposits { get; set; } = new List<Deposit>();

    public decimal TotalDeposits
    {
        get
        {
            decimal total = 0;
            foreach (var deposit in Deposits)
            {
                total += deposit.Amount;
            }
            return total;
        }
    }

    public Branch(string name)
    {
        Name = name;
    }
}

public abstract class Deposit
{
    public string DepositorName { get; set; }
    public decimal Amount { get; set; }

    protected Deposit(string depositorName, decimal amount)
    {
        DepositorName = depositorName;
        Amount = amount;
    }

    public abstract decimal CalculateTotalAmount(int months);
}

public class LongTermDeposit : Deposit
{
    public LongTermDeposit(string depositorName, decimal amount) : base(depositorName, amount) { }

    public override decimal CalculateTotalAmount(int months)
    {
        double interestRate = 0.05; // Годовая процентная ставка
        return Amount * (decimal)Math.Pow(1 + interestRate / 12, months);
    }
}

public class DemandDeposit : Deposit
{
    public DemandDeposit(string depositorName, decimal amount) : base(depositorName, amount) { }

    public override decimal CalculateTotalAmount(int months) => Amount; // Без начисления процентов
}

class Program
{
    static void Main(string[] args)
    {
        Bank bank = new Bank("Демо Банк");
        InitializeBank(bank);

        bool continueSearch = true;
        while (continueSearch)
        {
            Console.WriteLine("Хотите выполнить поиск в 'Демо Банке'? (y/n)");
            if (Console.ReadLine().Trim().ToLower() == "y")
            {
                PerformSearch(bank);
            }
            else
            {
                continueSearch = false;
                Console.WriteLine("До свидания!");
            }
        }
    }

    static void PerformSearch(Bank bank)
    {
        Console.WriteLine("Введите категорию для поиска (Банк, Филиал, Вклад):");
        string category = Console.ReadLine().Trim();
        switch (category.ToLower())
        {
            case "банк":
                bank.DisplayBankInfo();
                break;
            case "филиал":
                Console.WriteLine("Введите название филиала для поиска:");
                string branchName = Console.ReadLine().Trim().ToLower();
                foreach (var branch in bank.Branches)
                {
                    if (branch.Name.ToLower().Contains(branchName))
                    {
                        Console.WriteLine($"Филиал: {branch.Name}, Общая сумма вкладов: {branch.TotalDeposits:C}");
                    }
                }
                break;
            case "вклад":
                Console.WriteLine("Введите ФИО вкладчика для поиска:");
                string depositorName = Console.ReadLine().Trim().ToLower();
                bool found = false;
                foreach (var branch in bank.Branches)
                {
                    foreach (var deposit in branch.Deposits)
                    {
                        if (deposit.DepositorName.ToLower().Contains(depositorName))
                        {
                            Console.WriteLine($"Вкладчик: {deposit.DepositorName}, Филиал: {branch.Name}, Сумма вклада: {deposit.Amount:C}, Рассчитанная сумма за 12 месяцев: {deposit.CalculateTotalAmount(12):C}");
                            found = true;
                        }
                    }
                }
                if (!found)
                {
                    Console.WriteLine("Вкладчик не найден.");
                }
                break;
            default:
                Console.WriteLine("Неверное название категории.");
                break;
        }
    }

    static void InitializeBank(Bank bank)
    {
        Branch mainBranch = new Branch("Главный филиал");
        List<Deposit> mainDeposits = new List<Deposit> {
        new LongTermDeposit("Иван Иванов", 500000),
        new LongTermDeposit("Петр Петров", 600000),
        new LongTermDeposit("Светлана Иванова", 550000),
        new LongTermDeposit("Ольга Петрова", 580000),
        new LongTermDeposit("Александр Смирнов", 620000)
    };
        mainBranch.Deposits.AddRange(mainDeposits);
        bank.Branches.Add(mainBranch);
        bank.Deposits.AddRange(mainDeposits);  // Добавление вкладов в общий список банка

        Branch northBranch = new Branch("Северный филиал");
        List<Deposit> northDeposits = new List<Deposit> {
        new LongTermDeposit("Мария Сидорова", 700000),
        new LongTermDeposit("Николай Николаев", 720000),
        new LongTermDeposit("Елена Макарова", 710000),
        new LongTermDeposit("Дмитрий Орлов", 690000),
        new LongTermDeposit("Ирина Жукова", 680000)
    };
        northBranch.Deposits.AddRange(northDeposits);
        bank.Branches.Add(northBranch);
        bank.Deposits.AddRange(northDeposits);  // Добавление вкладов в общий список банка
    }
}
