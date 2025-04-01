using System;

namespace Bank.Accounts;

[Serializable]
public class InsufficientFundsException : ApplicationException
{
  public decimal? Needed { get; }
  public decimal? Actual { get; }

  // Constructor without a message
  public InsufficientFundsException(decimal taking, decimal balance)
      : base("Insufficient funds to complete the transaction.")
  {
    this.Needed = taking;
    this.Actual = balance;
  }

  // Constructor with a custom message
  public InsufficientFundsException(string message, decimal taking, decimal balance)
      : base(message)
  {
    this.Needed = taking;
    this.Actual = balance;
  }

  // Override ToString to provide a custom exception message
  public override string ToString()
  {
    return $"{base.ToString()}, Needed: {this.Needed}, Actual: {this.Actual}";
  }
}

public class Account
{
  public long id;
  public decimal Balance { get; protected set; }

  // Can throw InsufficientFundsException
  // Returns remaining balance on the account
  public virtual decimal Take(decimal amount)
  {
    if (amount > Balance)
    {
      throw new InsufficientFundsException(amount, this.Balance);
    }

    return this.Balance -= amount;
  }

  // Returns remaining balance on the account
  public virtual decimal Recieve(decimal amount)
  {
    return this.Balance += amount;
  }
  public virtual void MonthlyHook() { }
}

public class SavingsAccount : Account
{

}
