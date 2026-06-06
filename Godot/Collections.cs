using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Metarract.GameLib.Godot;
public static class Collections {
  public static T GetRandomItem<T>(T[] arr) => arr[Mathf.FloorToInt(GD.Randf() * arr.Length)];
  public static T GetRandomItem<T>(List<T> arr) => arr[Mathf.FloorToInt(GD.Randf() * arr.Count)];
  public static T GetRandom<T>(this List<T> arr) => arr[Mathf.FloorToInt(GD.Randf() * arr.Count)];

  public static T GetRandom<T>(this IEnumerable<T> arr) {
    T[] enumerable = arr as T[] ?? arr.ToArray();
    return enumerable.ElementAt(Mathf.FloorToInt(GD.Randf() * enumerable.Length));
  }
}
