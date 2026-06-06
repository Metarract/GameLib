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
  /// A standard LERP takes approximately 3x longer to be within 5% of target value by delta.
  /// This factor should be multiplied by the frame delta (assuming the frame delta is in seconds) to get a
  /// lerp timeline that is a factor of seconds
  /// </summary>
  public const double LERP_T_FCTR_5PCT = 3;

  public const double LERP_T_FCTR_10PCT = 2.283333333;
  
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
}
