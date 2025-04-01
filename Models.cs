
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
  public enum AccountType
  {
    Savings,
    SavingsStudent,
    Credit,
    Default,
  }

  public enum UserType
  {
    User,
    Banker,
    Admin,
  }

  public enum TransactionType
  {
    Transaction,

  }

  public class User
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long UID { get; set; }
    public string Name { get; set; }
    public string Hash { get; set; }
    public UserType Type { get; set; }

    public virtual ICollection<Account> Accounts { get; set; }
  }

  public class Account
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public decimal Balance { get; set; }
    public AccountType Type { get; set; }
  }

  public class Transaction
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime Time { get; set; }

    public decimal Amount { get; set; }

    public virtual Account? Source { get; set; }
    public virtual Account? Destination { get; set; }

    public TransactionType Type { get; set; }
  }
}
