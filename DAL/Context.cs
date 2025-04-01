
namespace Bank.DAL;
using Bank.Models;
using Microsoft.EntityFrameworkCore;

public class BankContext : DbContext
{
  public DbSet<User> Users { get; set; }
  public DbSet<Account> Accounts { get; set; }
  public DbSet<Transaction> Transactions { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    UI.Frontend.Log("Connecting to database", UI.LogLevel.Debug);
    optionsBuilder.UseSqlite("Data Source=bank.db");
  }
}


