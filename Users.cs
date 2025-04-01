namespace Bank;

using Bank.Models;
using BCrypt.Net;

public static class Users
{
  public static User? Register(string username, string password)
  {
    if(DAL.DAL.Instance.GetUser(username) != null)
      return null;
    string hash = BCrypt.HashPassword(password, 13);
    var user = new User { Name = username, Hash = hash };

    DAL.DAL.Instance.AddUser(user);

    return user;
  }

  public static User? Login(string username, string password)
  {
    var user = DAL.DAL.Instance.GetUser(username);
    if (user == null)
      return null;

    if (!BCrypt.Verify(password, user.Hash))
      return null;

    return user;
  }
}
