using DB_HW_Lesson4.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DB_HW_Lesson4
{
    public class DataContext: DbContext 
    {
        public DbSet<Client> clients { get; set; } 
        public DbSet<Deposit> deposits { get; set; }

        public DbSet<Withdrawal> withdrawals { get; set; }
        public DataContext() 
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost:5433;Username=postgres;Password=pass;Database=otus_HW");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
