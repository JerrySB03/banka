namespace Bank.DAL;

using Bank.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;


public class DAL
{
  private static readonly Lazy<DAL> _instance = new Lazy<DAL>(() => new DAL());
  private readonly BankContext _context;

  // Private constructor to prevent direct instantiation
  private DAL()
  {
    _context = new BankContext();
  }

  // Singleton instance access
  public static DAL Instance => _instance.Value;

  // Add a new User
  public void AddUser(User user)
  {
    _context.Users.Add(user);
    _context.SaveChanges();
  }

  // Get a User by ID
  public User? GetUser(long id)
  {
    return _context.Users.Include(u => u.Accounts).FirstOrDefault(u => u.UID == id);
  }
  public User? GetUser(string name)
  {
    return _context.Users.Include(u => u.Accounts).FirstOrDefault(u => u.Name == name);
  }

  // Get all Users
  public IQueryable<User> GetAllUsers()
  {
    return _context.Users.Include(u => u.Accounts);
  }

  // Update a User's details
  public void UpdateUser(User user)
  {
    var existingUser = _context.Users.FirstOrDefault(u => u.UID == user.UID);
    if (existingUser != null)
    {
      existingUser.Name = user.Name;
      existingUser.Hash = user.Hash;
      existingUser.Type = user.Type;
      _context.SaveChanges();
    }
  }

  // Delete a User
  public void DeleteUser(long id)
  {
    var user = _context.Users.Include(u => u.Accounts).FirstOrDefault(u => u.UID == id);
    if (user != null)
    {
      _context.Users.Remove(user);
      _context.SaveChanges();
    }
  }

  // Add a new Account
  public void AddAccount(Account account)
  {
    _context.Accounts.Add(account);
    _context.SaveChanges();
  }

  // Get an Account by ID
  public Account? GetAccount(long id)
  {
    return _context.Accounts.FirstOrDefault(a => a.ID == id);
  }

  // Get all Accounts
  public IQueryable<Account> GetAllAccounts()
  {
    return _context.Accounts;
  }

  // Update an Account's balance
  public void UpdateAccount(Account account)
  {
    var existingAccount = _context.Accounts.FirstOrDefault(a => a.ID == account.ID);
    if (existingAccount != null)
    {
      existingAccount.Balance = account.Balance;
      existingAccount.Type = account.Type;
      _context.SaveChanges();
    }
  }

  // Delete an Account
  public void DeleteAccount(long id)
  {
    var account = _context.Accounts.FirstOrDefault(a => a.ID == id);
    if (account != null)
    {
      _context.Accounts.Remove(account);
      _context.SaveChanges();
    }
  }

  // Add a new Transaction
  public void AddTransaction(Transaction transaction)
  {
    if(transaction.Source != null)
      transaction.Source.Balance -= transaction.Amount;

    if(transaction.Destination != null)
      transaction.Destination.Balance += transaction.Amount;

    _context.Transactions.Add(transaction);
    _context.SaveChanges();
  }

  // Get a Transaction by ID
  public Transaction? GetTransaction(long id)
  {
    return _context.Transactions.Include(t => t.Source).Include(t => t.Destination).FirstOrDefault(t => t.ID == id);
  }

  // Get all Transactions
  public IQueryable<Transaction> GetAllTransactions()
  {
    return _context.Transactions.Include(t => t.Source).Include(t => t.Destination);
  }
}
