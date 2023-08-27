

using DB_HW_Lesson4;
using DB_HW_Lesson4.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;


//const string connectionString = "Host=localhost:5433;Username=postgres;Password=pass;Database=otus_HW";


#region SberHW
static void AddRandomUsers()        //Генерирует рандомных юзеров
{
    Random random = new Random();

    List<string> names = new List<string>()                 //Необходимо для генерации имени и фамилии
{   "Павел ", "Василий", "Адам", "Иван", "Виктор", "Артемий",
    "Кирилл", "Матвей", "Тимофей", "Игорь", "Артём", "Максим ", "Михаил" };


    List<string> lastName = new List<string>()
{   "Крючков ", "Киселев", "Кириллов", "Кузнецов", "Куликов", "Лапин",
    "Лебедев", "Логинов", "Лукьянов", "Медведев", "Мельников", "Николаев ", "Панин" };


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
        foreach (Client client in clients) { Console.WriteLine($" {client.Id, -5} {client.Name,-10} {client.LastName,-10} {client.Balance,-10} {client.Email,-10} "); }
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
        }

    }
}



//AddRandomUsers();
GetAllClients();
Console.WriteLine();
DeleteClient(10);
Console.WriteLine();
GetAllClients();



#endregion

//#region MyRegion

//static void GetVersion()
//{
//    using (var connection = new NpgsqlConnection(connectionString))
//    {
//        connection.Open();

//        var sql = "SELECT version()";

//        using var cmd = new NpgsqlCommand(sql, connection);
//        var version = cmd.ExecuteScalar().ToString();
//        Console.WriteLine($"PostgreSQL version: {version}");
//    }
//}

//static void CreateTable(string tables)
//{
//    using (var connection = new NpgsqlConnection(connectionString))
//    {
//        connection.Open();

//        var sql = $"{tables}";

//        using var cmd = new NpgsqlCommand(sql, connection);

//        var affectedRowsCount = cmd.ExecuteNonQuery().ToString();

//        Console.WriteLine($"Created CLIENTS table. Affected rows count: {affectedRowsCount}");
//    }
//}

//static void AddData(string data)
//{
//    using (var connection = new NpgsqlConnection(connectionString))
//    {
//        connection.Open();

//        var sql = $"{data}";

//        using var cmd = new NpgsqlCommand(sql, connection);

//        var affectedRowsCount = cmd.ExecuteNonQuery().ToString();

//        Console.WriteLine($"Add data. Affected rows count: {affectedRowsCount}");
//    }
//}


////GetVersion();

////string newTables = @"
////CREATE SEQUENCE users_id;

////CREATE TABLE users
////(
////    id          BIGINT                      NOT NULL    DEFAULT NEXTVAL('users_id'),
////    name        CHARACTER VARYING(255)      NOT NULL,
////    surname     CHARACTER VARYING(255)      NOT NULL,
////    position    CHARACTER VARYING(255)      NOT NULL

////)
////";

////CreateTable(newTables);

//string innData = @"
//INSERT INTO users(name, surname, position) 
//VALUES ('Самуил', 'Аристархович', 'НИИ Алкотест');

//INSERT INTO users(name, surname, position) 
//VALUES ('Павел', 'Кондратьев', 'ПАО Фиолент');
//";
//AddData(innData);




//#endregion  

