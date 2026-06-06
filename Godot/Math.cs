using Godot;
using SMath = System.Math;

namespace Metarract.GameLib.Godot;
public static class Math {
  public static double Lerp2(double value, double target, double rate, double delta) {
    double powD = SMath.Pow(2, -rate * delta);
    return Mathf.Lerp(value, target, powD);
  }
}
