

using DB_HW_Lesson4;
using DB_HW_Lesson4.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;



#region SQL scripts 

const string connectionString = "Host=localhost:5433;Username=postgres;Password=pass;Database=otus_HW";

static void AddDepositsSQL()
{

    using (var connection = new NpgsqlConnection(connectionString))
    {
        connection.Open();

        var sql = $"INSERT INTO deposits " +
            $"\r\nVALUES(\r\n\t(SELECT max(deposits.id) from deposits) + 1, " +
            $"\r\n\tfloor(random() * 1000)," +
            $"\r\n\tfloor(random() * (SELECT max(clients.id) from clients)  " +
            $"\r\n))";

        using var cmd = new NpgsqlCommand(sql, connection);

        var affectedRowsCount = cmd.ExecuteNonQuery().ToString();

        Console.WriteLine($"Add data. Affected rows count: {affectedRowsCount}");
    }

}

static void AddWithdrawalsSQL()
{

    using (var connection = new NpgsqlConnection(connectionString))
    {
        connection.Open();

        var sql = $"INSERT INTO withdrawals " +
            $"\r\nVALUES(\r\n\t(SELECT max(withdrawals.id) from withdrawals) + 1, " +
            $"\r\n\tfloor(random() * 1000)," +
            $"\r\n\tfloor(random() * (SELECT max(clients.id) from clients)  " +
            $"\r\n))";

        using var cmd = new NpgsqlCommand(sql, connection);


        var affectedRowsCount = cmd.ExecuteNonQuery().ToString();

        Console.WriteLine($"Add data. Affected rows count: {affectedRowsCount}");
    }

}

static void AddClientssSQL()
{
    Random random = new Random();

    List<string> names = new List<string>()                 //Необходимо для генерации имени и фамилии
{   "Павел", "Василий", "Адам", "Иван", "Виктор", "Артемий",
    "Кирилл", "Матвей", "Тимофей", "Игорь", "Артём", "Максим", "Михаил" };


    List<string> lastName = new List<string>()
{   "Крючков", "Киселев", "Кириллов", "Кузнецов", "Куликов", "Лапин",
    "Лебедев", "Логинов", "Лукьянов", "Медведев", "Мельников", "Николаев", "Панин" };

    string surname = lastName[random.Next(lastName.Count)];

    using (var connection = new NpgsqlConnection(connectionString))
    {
        connection.Open();

        var sql = $"INSERT INTO clients VALUES((SELECT max(clients.id) from clients) + 1, " +
            $"'{names[random.Next(names.Count)]}', '{surname}', floor(random() * 1000), " +
            $"'{surname}{random.Next(1000)}@ya.ru')" ;

        using var cmd = new NpgsqlCommand(sql, connection);

        var affectedRowsCount = cmd.ExecuteNonQuery().ToString();

        Console.WriteLine($"Add data. Affected rows count: {affectedRowsCount}");
    }

}

static void AddData()
{
    for (int i = 0; i < 5; i++)
    {
        AddClientssSQL();
        AddDepositsSQL();
        AddWithdrawalsSQL();
    }
}


AddData();

#endregion

#region SberHW

static void ConsoleShow(Client client)
{
    Console.WriteLine($" {client.id,-5} {client.name,-10} {client.lastName,-10} {client.balance,-10} {client.email,-10} ");
}

static void AddRandomUsers()        //Генерирует рандомных юзеров
{
    Random random = new Random();

    List<string> names = new List<string>()                 //Необходимо для генерации имени и фамилии
{   "Павел", "Василий", "Адам", "Иван", "Виктор", "Артемий",
    "Кирилл", "Матвей", "Тимофей", "Игорь", "Артём", "Максим", "Михаил" };


    List<string> lastName = new List<string>()
{   "Крючков", "Киселев", "Кириллов", "Кузнецов", "Куликов", "Лапин",
    "Лебедев", "Логинов", "Лукьянов", "Медведев", "Мельников", "Николаев", "Панин" };


    List<Client> clients = new List<Client>();
    for (int i = 0; i < 10; i++)                                    //Создвем 10 рандомных юзеров
    {
        Client client = new Client();
        //client.Id = Guid.NewGuid();   Планировалось на гуидах, на в консоле добавить юзеру ID 10  сто рублей проще
        client.name = names[random.Next(names.Count)];
        client.lastName = lastName[random.Next(lastName.Count)];
        client.balance = random.Next(1000) * 1000;            //рандомный баланс с округлением до 1000
        client.email = client.lastName + random.Next(10000) + "@ya.ru";
        clients.Add(client);
    }

    foreach (Client client in clients)                   //выводим на консоль
    {
        Console.WriteLine($" {client.name} {client.lastName} {client.balance} {client.email} ");
    }

    foreach (Client client in clients)                  //записываем в бд
    {
        using (DataContext db = new DataContext())
        {
            db.clients.Add(client);
            db.SaveChanges();
        }
    }

}

static void GetAllClients()
{
    using (DataContext db = new DataContext())
    {
        var clients = db.clients.ToList();
        foreach (Client client in clients) { ConsoleShow(client); }
    }

}

static void GetAllWithdrow()
{
    using (DataContext db = new DataContext())
    {
        var withds = db.withdrawals.ToList();
        foreach (Withdrawal withd in withds) { Console.WriteLine($"Id транзакции {withd.id} клиент с ID {withd.clientId} снял {withd.amount}"); };
    }

}

static void GetAllDeposits()
{
    using (DataContext db = new DataContext())
    {
        var deposits = db.deposits.ToList();
        foreach (Deposit deposit in deposits) { Console.WriteLine($"Id транзакции {deposit.id} клиент с ID {deposit.clientId} положил {deposit.amount}"); };
    }

}
static void DeleteClient(int clientId)
{
    using (DataContext db = new DataContext())
    {
        var client = db.clients.FirstOrDefault(x => x.id == clientId);
        if (client != null)
        {
            db.clients.Remove(client);
            db.SaveChanges();
            Console.WriteLine($"{client.id} удален");

        }
        else { Console.WriteLine("Что то пошло не так"); }

    }
}

static void GetOne(int clientId)
{
    using (DataContext db = new DataContext())
    {
        var client = db.clients.FirstOrDefault(x => x.id == clientId);
        if (client != null)
        {
            ConsoleShow(client);
            Console.WriteLine();

        }
        else { Console.WriteLine("Клиент не найден"); }
    }

}

static void Deposit(int clientId, decimal amount)
{
    using (DataContext db = new DataContext())
    {
        var client = db.clients.FirstOrDefault(c => c.id == clientId);
        if (client != null)
        {
            Deposit deposit = new Deposit();
            deposit.Client = client;
            deposit.amount = amount;
            client.balance = client.balance + deposit.amount;
            db.deposits.Add(deposit);


            db.SaveChanges();
            Console.WriteLine($"Клиент {client.id} {client.name} {client.lastName}  положил {deposit.amount} на свой счет");
            ConsoleShow(client);
            Console.WriteLine();

        }
        else
        {
            Console.WriteLine("Клиента с таким ID нет");
        }
    }
}

static void Withdraw(int clientId, decimal amount)
{
    using (DataContext db = new DataContext())
    {

        var client = db.clients.FirstOrDefault(c => c.id == clientId);
        if (client != null)
        {
            Withdrawal wthdraw = new Withdrawal();
            wthdraw.Client = client;
            wthdraw.amount = amount;
            if (client.balance >= amount)
            {
                client.balance = client.balance - wthdraw.amount;
                db.withdrawals.Add(wthdraw);
                db.SaveChanges();

                Console.WriteLine($"Клиент {client.id} {client.name} {client.lastName}  снял {wthdraw.amount} со своего счета");
                ConsoleShow(client);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($" Клиент {client.id} {client.name} {client.lastName} не может снять {wthdraw.amount}, на счету клиента {client.balance}");
            }


        }
        else
        {
            Console.WriteLine("Клиента с таким ID нет");
        }
    }
}

Console.WriteLine("Что хотим? Получить список команд - help");
bool exit = false;

static void RandDeposit()       //генерируем 5 записей в таблицу депозитов
{
    Random rnd = new Random();
    int count;
    using (DataContext db = new DataContext())
    {
        var clients = db.clients.ToList();
        count = clients.Count;
    }
    if (count > 0)
    {
        for (int i = 0; i < 5; i++)
        {
            Deposit(rnd.Next(count), rnd.Next(50) * 1000);
        }
    }
    else
    {
        Console.WriteLine("В банке нет пользвоателей");
    }

}

static void RandWithdraw() //генерируем 5 записей в таблицу выводов
{
    Random rnd = new Random();
    int count;
    using (DataContext db = new DataContext())
    {
        var clients = db.clients.ToList();
        count = clients.Count;
    }
    if (count > 0)
    {
        for (int i = 0; i < 5; i++)
        {
            Withdraw(rnd.Next(count), rnd.Next(50) * 1000);
        }
    }
    else
    {
        Console.WriteLine("В банке нет пользвоателей");
    }

}

while (!exit)
{
    string command = Console.ReadLine().ToString();
    int clientId = new int();
    decimal amaunt = new decimal();
    Random rnd = new Random();
    int count;
    switch (command)
    {
        case "add":
            AddRandomUsers();
            break;
        case "allcl":
            GetAllClients();
            break;
        case "alldepo":
            GetAllDeposits();
            break;
        case "allwith":
            GetAllWithdrow();
            break;
        case "get":
            Console.WriteLine("Id клиента");
            clientId = int.Parse(Console.ReadLine());
            GetOne(clientId);
            break;
        case "del":
            Console.WriteLine("Id клиента");
            clientId = int.Parse(Console.ReadLine());
            DeleteClient(clientId);
            break;
        case "depo":
            Console.WriteLine("Кому положить");
            clientId = int.Parse(Console.ReadLine());
            Console.WriteLine("Сколько положить");
            amaunt = decimal.Parse(Console.ReadLine());
            Deposit(clientId, amaunt);
            break;
        case "randDepo":
            RandDeposit();
            break;
        case "randWith":
            RandWithdraw();
            break;
        case "with":
            Console.WriteLine("С кого снять");
            clientId = int.Parse(Console.ReadLine());
            Console.WriteLine("Сколько снять");
            amaunt = decimal.Parse(Console.ReadLine());
            Withdraw(clientId, amaunt);
            break;
        case "help":
            Console.WriteLine("выход  - exit , depo, with, allcl, alldepo, allwith, get, del  ");
            Console.WriteLine(" add - добавть 10 клиентов, randWith - добавить 5 транзакций в вывод, randDepo - добавить 5 транзакций в депозит");
            break;
        case "exit":
            exit = true;
            break;
        default:
            Console.WriteLine("Неизвестная команда, что б получить список команд напишите help");
            break;
    }
}
#endregion

