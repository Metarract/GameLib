using Godot;

namespace Metarract.GameLib.Godot;
public static class Transform {
  private static float _jitterAngle;

  /// <summary>
  /// JitterAngle is incremented by an approximation of an irrational number based on the golden ratio, such that each
  /// time it is referenced it should have as little overlap with the previous iteration as possible 
  /// </summary>
  /// <remarks>
  /// Assumes radians
  /// </remarks>
  public static float JitterAngle {
    get {
      _jitterAngle += Standard.Math.GOLDEN_ANGLE;
      _jitterAngle = Mathf.PosMod(_jitterAngle, Mathf.Tau);
      return _jitterAngle;
    }
  }
  
  public static void JitterPosition(ref Vector2 position) => position += Vector2.Right.Rotated(JitterAngle);
}
