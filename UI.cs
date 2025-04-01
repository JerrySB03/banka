namespace Bank.UI;

using System.ComponentModel;
using Bank.Models;
using Spectre.Console;

public enum LogLevel
{
  Info,
  Debug,
  Error,
}

public static class Frontend
{
  public enum Selection
  {
    LoginUsername,
    LoginPassword,
  }
  static void CycleSelection()
  {
    selected = selected switch
    {
      Selection.LoginUsername =>
        Selection.LoginPassword,
      Selection.LoginPassword =>
        Selection.LoginUsername,
    };
  }
  static User? loggedUser = null;
  static string username = String.Empty;
  static string password = String.Empty;
  const string cursor = "█";

  static Selection selected = Selection.LoginUsername;

  static List<(DateTime time, string message, LogLevel level)> logs = new List<(DateTime, string, LogLevel)> { (DateTime.Now, "Banka started", LogLevel.Info) };
  static public void Log(string message, LogLevel level)
  {
    logs.Add((DateTime.Now, message, level));
  }
  static CancellationTokenSource cts = new CancellationTokenSource();

  private static string RenderInputBox(string name, string value, int size, bool isSelected = false)
  {
    if (isSelected)
      value = value.Substring(Math.Max(value.Length - size, 0)) + $"[red]{cursor}[/]";
    else
      value = value.Substring(Math.Max(value.Length - size, 0));
    return $"{name}: {value.PadRight(size)}";

  }
  private static void RenderLogin(LiveDisplayContext ctx)
  {
    var root = new Layout("Root")
        .SplitColumns(
            new Layout("Left"),
            new Layout("Middle"),
            new Layout("Right")
        );

    var login = Align.Center(new Panel(new Markup(
@$"[bold]Login[/]

{RenderInputBox("Username", username, 32, selected == Selection.LoginUsername)}
{RenderInputBox("Password", new String('*', password.Length), 32, selected == Selection.LoginPassword)}

[red]TAB[/] to switch input fields [red]ENTER[/] to confirm"
              )), VerticalAlignment.Middle);

    var logPanel = Align.Right(new Panel(new Markup(RenderLogs())), VerticalAlignment.Bottom);

    root["Left"].Update(new Markup(""));
    root["Middle"].Update(login);
    root["Right"].Update(logPanel);

    ctx.UpdateTarget(root);
    ctx.Refresh();
  }
  private static void RenderMenu(LiveDisplayContext ctx)
  {

  }
  public static async Task Render()
  {
    var renderTask = RenderLoop(cts.Token);

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


  static string RenderLogs()
  {
    var output = String.Empty;
    foreach (var log in logs)
    {
      output += $"\n{log.time.ToShortTimeString()}|{log.level.ToString()[0]}|{log.message}";
    }
    return output;
  }
  static async Task RenderLoop(CancellationToken token)
  {

    await AnsiConsole.Live(new Layout("Root")
    .SplitColumns(
        new Layout("Left"),
        new Layout("Middle").SplitRows(new Layout("Middle Top"),
          new Layout("Middle Center"),
          new Layout("Middle Bottom")),
        new Layout("Right")
        .SplitRows(
          new Layout("Right Top"),
          new Layout("Right Bottom"))
    )).StartAsync(async ctx =>
    {
      while (!token.IsCancellationRequested)
      {
        try
        {
          await Task.Delay(10);

          if (loggedUser == null)
            RenderLogin(ctx);
          else
            RenderMenu(ctx);
        }
        catch (Exception e)
        {
          Console.Error.WriteLine(e.ToString());
          logs.Add((DateTime.Now, e.ToString(), LogLevel.Error));
        }
      }
    });
  }
  private static void Login()
  {
    Log( $"Attempting to login as {username}", LogLevel.Debug);
    loggedUser = Users.Login(username, password);
    if (loggedUser == null)
      Log($"Failed to login as {username}", LogLevel.Info);
    else
     Log($"Logged in as {username}", LogLevel.Info);

  }
  private static void HandleKeyPress(ConsoleKeyInfo key)
  {
    if (key.Key == ConsoleKey.Tab)
    {
      CycleSelection();
      return;
    }
    switch (selected)
    {
      case Selection.LoginUsername:
        if (key.Key == ConsoleKey.Enter)
          Login();
        else if (key.Key == ConsoleKey.Backspace)
        {
          if (username.Length > 0)
            username = username.Substring(0, username.Length - 1);
        }
        else
          username += key.KeyChar;
        break;
      case Selection.LoginPassword:
        if (key.Key == ConsoleKey.Enter)
          Login();
        else if (key.Key == ConsoleKey.Backspace)
        {
          if (password.Length > 0)
            password = password.Substring(0, password.Length - 1);
        }
        else
          password += key.KeyChar;
        break;
      default:
        break;
    }
  }
}
