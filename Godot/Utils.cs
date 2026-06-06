using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metarract.GameLib.Godot;
public static class Utils {
  #region Randomization and Trending


  /// <summary>
  /// simple check to see if our number gets hit, chance /should/ be a value from 0-1.
  /// If the input chance is greater than or equal to 1 it is always true,
  /// if it is less than or equal to 0 it is always false.
  ///
  /// example: 0.2 -> 20% chance
  /// </summary>
  public static bool TryChance(this float chance) => GD.Randf() < chance;

  /// <summary>
  /// Generates a random number and maps it to the provided range.
  /// e.g.: given a range from 0 to 100, if our randomly generated number is 0.2f, then the result is 20.
  /// </summary>
  public static float MapRandToRange((float, float) range) => Mathf.Remap(GD.Randf(), 0, 1, range.Item1, range.Item2);

  /// <summary>
  /// given an easing curve value, maps to a provided range or returns the eased amount from 0-1 based on the provided trend curve
  /// </summary>
  /// <seealso>https://raw.githubusercontent.com/godotengine/godot-docs/master/img/ease_cheatsheet.png</seealso>
  public static double TrendValue(float easingCurve, (double, double) remapRange = default) {
    var eased = Mathf.Ease(GD.Randf(), easingCurve);

    if (remapRange.Equals(default))
      return eased; // if our maprange is the default, we only wanted 0-1 anyway so return our eased value
    return Mathf.Remap(eased, 0, 1, remapRange.Item1, remapRange.Item2);
  }
  #endregion

  #region Instantiation Utilities
  public static T GetSceneInstance<T>(StringName sceneLocation) where T : Node {
    var pck = GD.Load<PackedScene>(sceneLocation);
    return pck.Instantiate<T>();
  }

  public static GodotObject InstantiateGDScript(string path) {
    var gdScript = GD.Load<GDScript>(path);
    return (GodotObject)gdScript.New();
  }
  #endregion

  /// <summary>
  /// Quick and dirty awaiter for a node
  /// </summary>
  public static SignalAwaiter GetAwaiter(this Node node, float delayInSeconds) =>
    node.ToSignal(node.GetTree().CreateTimer(delayInSeconds), SceneTreeTimer.SignalName.Timeout);
  // public static SignalAwaiter GetCallbackTimer(this Node node, float delayInSeconds, Action action) => node.ToSignal(node.GetTree().CreateTimer(delayInSeconds), action);

  #region common lifecycles passthroughs
  /// <summary>
  /// To be used from any UnhandledInput or Input call
  /// </summary>
  public static void OnMouseClick(InputEvent inputEvent, MouseButtonMask button, Action action) {
    if (inputEvent is not InputEventMouseButton mBtnEvent) return;
    if (!mBtnEvent.IsPressed() || mBtnEvent.ButtonMask != button) return;
    action();
  }
  #endregion
}


