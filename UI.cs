namespace Bank.UI;

using Bank.Models;
using Spectre.Console;

public enum LogLevel{
  Info,
  Debug,
  Error,
}

public static class Frontend
{
public enum Selection {
  LoginUsername,
  LoginPassword,
}
  static void CycleSelection(){
    selected = selected switch 
    {
      Selection.LoginUsername =>
        Selection.LoginPassword,
      Selection.LoginPassword =>
        Selection.LoginUsername,
    };
    Console.Error.WriteLine(selected);
  }
  static User? loggedUser = null;
  static string username = String.Empty;
  static string password = String.Empty;
  const string cursor = "█";

  static Selection selected = Selection.LoginUsername;

  static List<(DateTime time, string message, LogLevel level)> logs = new List<(DateTime, string, LogLevel)> { (DateTime.Now, "Banka started", LogLevel.Info) };
  static CancellationTokenSource cts = new CancellationTokenSource();

  private static string RenderInputBox(string name, string value, int size, bool isSelected = false){
    if(isSelected)
      value = value.Substring(Math.Max(value.Length - size, 0)) + $"[red]{cursor}[/]";
    else
      value = value.Substring(Math.Max(value.Length - size, 0));
return $"{name}: {value.PadRight(size)}";

  }
  private static void RenderLogin(LiveDisplayContext ctx)
  {
    var root = new Layout("Root");
    var middle = new Layout("Middle");

    var login = Align.Center(new Panel(new Markup(
@$"[bold]Login[/]

{RenderInputBox("Username", username, 32, selected == Selection.LoginUsername)}
{RenderInputBox("Password", new String('*', password.Length), 32, selected == Selection.LoginPassword)}

[red]TAB[/] to switch input fields [red]ENTER[/] to confirm"
              )), VerticalAlignment.Middle);


    middle.Update(login);

    ctx.UpdateTarget(middle);
    ctx.Refresh();
  }
  private static void RenderMenu(LiveDisplayContext ctx)
  {

  }
  public static async Task Render()
  {
    RenderLoop(cts.Token);
      // Start listening for key inputs in the main thread
      while (true)
      {
        if (Console.KeyAvailable)
        {
          ConsoleKeyInfo key = Console.ReadKey(intercept: true);
          HandleKeyPress(key);
        }
        await Task.Delay(10);
      }

  }


    public static async Task RenderLoop(CancellationToken token)
  {

    await AnsiConsole.Live(new Layout("Root")
    .SplitColumns(
        new Layout("Left"),
        new Layout("Middle").SplitRows(new Layout("Middle Top"),
          new Layout("Middle center"),
          new Layout("Middle Bottom")),
        new Layout("Right")
        .SplitRows(
          new Layout("Right Top"),
          new Layout("Right Bottom"))
    )).StartAsync(ctx =>
    {
      while (!token.IsCancellationRequested)
      {
        try
        {
          if (loggedUser == null)
            RenderLogin(ctx);
          else
            RenderMenu(ctx);
        }
        catch (Exception e)
        {
          Console.WriteLine(e.ToString());
          logs.Append((DateTime.Now, e.ToString(), LogLevel.Error));
        }
      }

      return Task.CompletedTask;
    });
      return Task.CompletedTask;
  }
    private static void HandleKeyPress(ConsoleKeyInfo key)
    {
      if(key.Key == ConsoleKey.Tab){
        CycleSelection();
        return;
      }
      switch (selected)
      {
        case Selection.LoginUsername:
          if(key.Key == ConsoleKey.Enter)
            loggedUser = Users.Login(username, password);
        else
          username += key.KeyChar;
        break;
        case Selection.LoginPassword:
          if(key.Key == ConsoleKey.Enter)
            loggedUser = Users.Login(username, password);
        else
          password += key.KeyChar;

          break;
          default:
            break;
      }
    }
}
