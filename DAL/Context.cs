
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
    optionsBuilder.UseSqlite("Data Source=bank.db");
  }
}


