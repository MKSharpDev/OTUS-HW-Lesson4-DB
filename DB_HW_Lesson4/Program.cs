

using DB_HW_Lesson4;
using DB_HW_Lesson4.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;




#region SberHW

static void ConsoleShow(Client client)
{
    Console.WriteLine($" {client.Id,-5} {client.Name,-10} {client.LastName,-10} {client.Balance,-10} {client.Email,-10} ");
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
        client.Name = names[random.Next(names.Count)];
        client.LastName = lastName[random.Next(lastName.Count )];
        client.Balance = random.Next(1000)*1000;            //рандомный баланс с округлением до 1000
        client.Email = client.LastName + random.Next(10000) + "@ya.ru";
        clients.Add(client);
    }

    foreach(Client client in clients)                   //выводим на консоль
    {
        Console.WriteLine($" {client.Name} {client.LastName} {client.Balance} {client.Email} ");
    }

    foreach (Client client in clients)                  //записываем в бд
    {
        using(DataContext db = new DataContext())
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
        foreach (Withdrawal withd in withds) { Console.WriteLine($"Id транзакции {withd.Id} клиент с ID {withd.ClientId} снял {withd.Amount}"); };
    }

}

static void GetAllDeposits()
{
    using (DataContext db = new DataContext())
    {
        var deposits = db.deposits.ToList();
        foreach (Deposit deposit in deposits) { Console.WriteLine($"Id транзакции {deposit.Id} клиент с ID {deposit.ClientId} положил {deposit.Amount}"); };
    }

}
static void DeleteClient(int clientId)
{
    using (DataContext db = new DataContext())
    {
        var client = db.clients.FirstOrDefault(x => x.Id == clientId);
        if (client != null)
        {
            db.clients.Remove(client);
            db.SaveChanges();
            Console.WriteLine($"{client.Id} удален");

        }
        else { Console.WriteLine("Что то пошло не так"); }

    }
}

static void GetOne(int clientId)
{
    using (DataContext db = new DataContext()) 
    { 
        var client = db.clients.FirstOrDefault(x => x.Id == clientId);
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
    using(DataContext db = new DataContext())
    {
        var client = db.clients.FirstOrDefault(c => c.Id == clientId);
        if (client != null)
        {
            Deposit deposit = new Deposit();
            deposit.Client = client;
            deposit.Amount = amount;
            client.Balance = client.Balance + deposit.Amount;
            db.deposits.Add(deposit);


            db.SaveChanges();
            Console.WriteLine($"Клиент {client.Id} {client.Name} {client.LastName}  положил {deposit.Amount} на свой счет");
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

        var client = db.clients.FirstOrDefault(c => c.Id == clientId);
        if (client != null)
        {
            Withdrawal wthdraw = new Withdrawal();
            wthdraw.Client = client;
            wthdraw.Amount = amount;
            if (client.Balance >= amount)
            {
                client.Balance = client.Balance - wthdraw.Amount;
                db.withdrawals.Add(wthdraw);
                db.SaveChanges();

                Console.WriteLine($"Клиент {client.Id} {client.Name} {client.LastName}  снял {wthdraw.Amount} со своего счета");
                ConsoleShow(client);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($" Клиент {client.Id} {client.Name} {client.LastName} не может снять {wthdraw.Amount}, на счету клиента {client.Balance}");
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
while (!exit)
{
    string  command = Console.ReadLine().ToString();
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
            using(DataContext db = new DataContext())
            {
                var clients = db.clients.ToList();
                count = clients.Count;
            }
            for (int i = 0; i < 5; i++)
            {
                Deposit(rnd.Next(count), rnd.Next(50)*1000);
            }
            break;
        case "randWith":
            using (DataContext db = new DataContext())
            {
                var clients = db.clients.ToList();
                count = clients.Count;
            }
            for (int i = 0; i < 5; i++)
            {
                Withdraw(rnd.Next(count), rnd.Next(50) * 1000);
            }
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

