using SMath = System.Math;

namespace Metarract.GameLib.Standard;
public static class Math {
  /// <summary>
  /// phi approximation
  /// </summary>
  public const double PHI = 1.618033988;

  /// <summary>
  /// Relative angle in radians to rotate an item by in order to have as little overlap as possible with the previous item 
  /// </summary>
  public const float GOLDEN_ANGLE = 2.399828f;

  /// <summary>
  /// Frame-independent lerp method. 
  /// Sourced from the amazing <see href="https://www.youtube.com/watch?v=LSNQuFEDOyQ">Freya Holmér</see>
  /// </summary>
  /// <remarks>
  /// Freya notes that a useful range for decayRate should be from 1-25, 1 being slow and 25 being fast
  /// </remarks>
  public static double Lerp2(double value, double target, double decayRate, double frameDeltaS) {
    // pow is slow so we use natural exponential function instead 
    return target + (value - target) * SMath.Exp(-decayRate * frameDeltaS);
  }
}
