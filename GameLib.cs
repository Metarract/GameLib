using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Metarract;
public static class Std {
  private static readonly Random Rng = new();

  #region extension methods
  public static void Each<T>(this IEnumerable<T> ie, Action<T, int> action) {
    var i = 0;
    foreach (var e in ie) action(e, i++);
  }

  /// <summary>
  /// shuffles list items in-place. lifted this from somewhere
  /// </summary>
  /// <typeparam name="T">list type</typeparam>
  /// <param name="list">list to be shuffled</param>
  public static void Shuffle<T>(this IList<T> list) {
    var n = list.Count;
    while (n > 1) {
      n--;
      var k = Rng.Next(n + 1);
      (list[n], list[k]) = (list[k], list[n]);
    }
  }

  public static (TB, TA) Reverse<TA, TB>(this (TA, TB) tuple) => (tuple.Item2, tuple.Item1);

  public static T DeepCopy<T>(this T obj) {
    using var memStream = new MemoryStream();
    if (!obj.GetType().IsSerializable) throw new NotSupportedException();
    var serializer = new XmlSerializer(typeof(T));
    serializer.Serialize(memStream, obj);
    memStream.Position = 0;

    return (T)serializer.Deserialize(memStream);
  }
  #endregion

  public static long UnixSecNow => new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
  public static long UnixMSecNow => new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();

  public static async void DoOnTimeout(int timeoutMs, Action act) {
    await Task.Delay(timeoutMs);
    act();
  }
}
