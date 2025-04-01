using System;


namespace Bank
{
  public static class Program
  {
    public static async Task Main()
    {
      await UI.Frontend.Render();
    }
  }
}
