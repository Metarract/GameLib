using Godot;

namespace Metarract.GameLib.Godot;
/// <summary>
/// Simple class for doing multiple checks on a random chance. The chance starts off at the given percentage (which
/// should be between 0 and 1) and gradually increases by a factor of <c>ScaleStepCurve</c> and the amount of checks
/// </summary>
public class ScalingRandom {
  // TODO calculate based off of exported easing value
  // this should start off by increasing the amount by basically nill, but scale as a factor of the chance
  // E.G.:
  // 10% chance: check 1 -> + 0. check 7 -> + 0.1, check 9 -> + 0.5
  // the closer we are in number of checks to the percentage, the higher the actual chance should scale
  
  /// <summary>
  /// An easing curve to adjust how much each check adjusts the chance, after each check. Use <c>[Export(PropertyHint.ExpEasing)]</c> on a
  /// property to see an easy-to-use curve editor in the UI and easily get a value for this
  /// <list type="bullet">
  /// <item>
  ///     <see href="https://docs.godotengine.org/en/stable/classes/class_%40globalscope.html#class-globalscope-method-ease">Ease method documentation</see>
  ///   </item>
  ///   <item>
  ///     <see href="https://raw.githubusercontent.com/godotengine/godot-docs/master/img/ease_cheatsheet.png">Easing Cheatsheet</see>
  ///   </item>
  /// </list>
  /// </summary>
  public float ScaleStepCurve { get; set; } = 4.8f;

  private uint _checkCount;
  private float _expectedCheckCount;
  private float _chanceOffset;
  public float CalculatedChance => Chance + _chanceOffset;
  public float Chance { get; }

  public ScalingRandom(float chance) {
    Chance = chance;
    _expectedCheckCount = 1f / chance;
  }

  public bool Check() {
    if (GD.Randf() < CalculatedChance) {
      Reset();
      return true;
    }
    
    IncrementChance();
    return false;
  }
  
  public void Reset() => _checkCount = 0;

  private void IncrementChance() {
    _checkCount++;
    _chanceOffset += Mathf.Ease(_checkCount / _expectedCheckCount, ScaleStepCurve);
  }
}
