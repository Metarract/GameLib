using System;
using System.Threading.Tasks;

namespace Metarract.GameLib.Standard;
public static class Time {
  public static long UnixSecNow => new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
  public static long UnixMSecNow => new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();

  public static async void DoOnTimeout(int timeoutMs, Action act) {
    try {
      await Task.Delay(timeoutMs);
      act();
    } catch (Exception e) {
      Console.Error.WriteLine(e);
    }
  }
}
