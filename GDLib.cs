using Godot;
using System.Reflection;

namespace GameLib.Godot;
public static class Std {
  /// <summary>
  /// a standard LERP takes approximately 3x longer to be within 5% of target value by delta.
  /// within 10% is around 2.2833_. 
  /// these are simply approximation values to make this easier on myself.
  /// </summary>
  public const double LERP_T_FCTR_5PCT = 3;
  public const double LERP_T_FCTR_10PCT = 2.283333333;

  public static double Lerp2(double value, double target, double rate, double delta) {
    var powD = Math.Pow(2, -rate * delta);
    return Mathf.Lerp(value, target, powD);
  }

  #region Debugging Tools
  public static void DrawLine(Node parent, List<Vector2> points, string color = "red", float width = 1) {
    var line = new Line2D {
      DefaultColor = new Color(color),
      Points = [.. points],
      Width = width
    };
    parent.AddChild(line);
  }
  #endregion

  #region Randomization and Trending
  public static T GetRandomItem<T>(T[] arr) => arr[Mathf.FloorToInt(GD.Randf() * arr.Length)];
  public static T GetRandomItem<T>(List<T> arr) => arr[Mathf.FloorToInt(GD.Randf() * arr.Count)];
  public static T GetRandom<T>(this List<T> arr) => arr[Mathf.FloorToInt(GD.Randf() * arr.Count)];
  /// <summary>
  /// simple check to see if our number gets hit, chance /should/ be a value from 0-1.
  /// If the input chance is greater than or equal to 1 it is always true,
  /// if it is less than or equal to 0 it is always false.
  ///
  /// example: 0.2 -> 20% chance
  /// </summary>
  public static bool TryChance(float chance) => GD.Randf() < chance; // our random number should be less than our chance to count as triggered

  /// <summary>
  /// Generates a random number and maps it to the provided range.
  /// e.g.: given a range from 0 to 100, if our randomly generated number is 0.2f, then the result is 20.
  /// </summary>
  public static float MapRandToRange((float, float) range) => Mathf.Remap(GD.Randf(), 0, 1, range.Item1, range.Item2);

  /// <summary>
  /// given an easing curve value, maps to a provided range or returns the eased amount from 0-1 based on the provided trend curve
  /// <see>https://raw.githubusercontent.com/godotengine/godot-docs/master/img/ease_cheatsheet.png</see>
  /// </summary>
  public static double TrendValue(float easingCurve, (double, double) remapRange = default) {
    var eased = Mathf.Ease(GD.Randf(), easingCurve);

    if (remapRange.Equals(default)) return eased; // if our maprange is the default, we only wanted 0-1 anyway so return our eased value
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
  public static SignalAwaiter GetAwaiter(this Node node, float delayInSeconds) => node.ToSignal(node.GetTree().CreateTimer(delayInSeconds), SceneTreeTimer.SignalName.Timeout);
  // public static SignalAwaiter GetCallbackTimer(this Node node, float delayInSeconds, Action action) => node.ToSignal(node.GetTree().CreateTimer(delayInSeconds), action);

  /// <summary>
  /// Simple / Clean method of getting child nodes for props/fields at Ready
  /// | Nodes should have the custom attribute [Node("#node/path/here")], with this method in the _Ready() function
  /// </summary>
  public static void WireNodes(this Node node) {
    // TODO -> that CANNOT be used for properties, they don't show up in this Get V
    // maybe investigate how to properly get that info or do something differently
    // for now just leave all as fields
    FieldInfo[] fieldInfo = node
      .GetType()
      .GetFields(
        BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
      );
    foreach (var field in fieldInfo) {
      var attribute = (NodeAttribute)Attribute.GetCustomAttribute(field, typeof(NodeAttribute));
      if (attribute is null) continue;
      field.SetValue(node, node.GetNode(attribute.NodePath));
    }
  }

  #region common lifecycles passthroughs
  public static void OnMouseClick(InputEvent inputEvent, MouseButtonMask button, Action action) {
    if (inputEvent is not InputEventMouseButton mBtnEvent) return;
    if (!mBtnEvent.IsPressed() || mBtnEvent.ButtonMask != button) return;
    action();
  }
  #endregion
}

#region attributes
[AttributeUsage(AttributeTargets.Field)]
public class NodeAttribute(string nodePath) : Attribute {
  private readonly string _nodePath = nodePath;
  public virtual string NodePath => _nodePath;
}
#endregion


/* NOTES
  a standard LERP takes approximately 3x longer to reach 5% of target value by delta
    e.g.: mathf.lerp(0, 100, delta / 3)
    delta / 3 implies "3 seconds"
    time to within 5% will be approx 9

  private bool DO_LERP_TEST;
  private double LERP_TARGET;
  private double LERP_CURRENT;
  private double LERP_TIME_DIV;
  private ulong LERP_TIMESTAMP;
  private void SetLerpCheck(double target) {
    LERP_CURRENT = 0;
    LERP_TARGET = target;
    LERP_TIME_DIV++;
    DO_LERP_TEST = true;
    LERP_TIMESTAMP = Time.GetTicksMsec();
    GD.Print("-----");
    GD.PrintS("expected s:", LERP_TIME_DIV);
  }
  private void CheckLerp(double delta) {
    if (!DO_LERP_TEST) return;
    LERP_CURRENT = Mathf.Lerp(LERP_CURRENT, LERP_TARGET, delta / LERP_TIME_DIV * 3);
    // 5 percent:
    // 1 -> ~3
    // 2 -> ~6
    // 3 -> ~9
    // 4 -> ~12
    // 5 -> ~15
    // 6 -> ~18
    // 10 percent:
    // 1 -> 2.2
    // 2 -> 4.5
    // 3 -> 6.8
    // 4 -> 9.1
    // 5 -> 11.4
    // 6 -> 13.7
    // 5 percent * 3:
    // 1 -> ~1
    // 2 -> ~2
    // 3 -> ~3
    // 4 -> ~4
    // 5 -> ~5
    // 6 -> ~6
    var approx = LERP_TARGET * 0.05;
    if (LERP_CURRENT + approx >= LERP_TARGET) { // are we within our percentage
      DO_LERP_TEST = false;
      var dt = Time.GetTicksMsec() - LERP_TIMESTAMP;
      GD.PrintS("elapsed ms:", dt, "s:", dt / 1000f);
    }
  }
*/

