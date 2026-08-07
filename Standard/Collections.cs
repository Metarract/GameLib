using System;
using System.Collections.Generic;

namespace Metarract.GameLib.Standard.Collections;
public static class Collections {
  private static readonly Random Rng = new();
  /// <summary>
  /// shuffles list items in-place. lifted this from somewhere
  /// </summary>
  public static void Shuffle<T>(this IList<T> list) {
    int n = list.Count;
    while (n > 1) {
      n--;
      int k = Rng.Next(n + 1);
      (list[n], list[k]) = (list[k], list[n]);
    }
  }
  
  /// <summary>
  /// ForEach with Index
  /// </summary>
  public static void Each<T>(this IEnumerable<T> collection, Action<T, int> action) {
    int i = 0;
    foreach (T e in collection) action(e, i++);
  }
}
