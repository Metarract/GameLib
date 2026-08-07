using Godot;

namespace Metarract.GameLib.Godot;
public static class Math {
  /// <summary>
  /// Utilizes Godot's Ease method (which only accepts values from 0-1) to remap a value from one range to another, according to the provided curve.
  /// All the normal behaviour for the remap method applies here, please see Godot's
  /// <see href="https://docs.godotengine.org/en/stable/classes/class_%40globalscope.html#class-globalscope-method-remap">remap method</see>
  /// for more information.
  /// </summary>
  /// <param name="value">The input value</param>
  /// <param name="inStart">The input's lower bound</param>
  /// <param name="inStop">The input's upper bound</param>
  /// <param name="outStart">The output's lower bound</param>
  /// <param name="outStop">The output's upper bound</param>
  /// <param name="easingCurve">The curve to apply. See Godot's <see href="https://docs.godotengine.org/en/stable/classes/class_%40globalscope.html#class-globalscope-method-ease">ease method</see> for more information</param>
  /// <param name="attenuate">Whether to attenuate the value along the curve or not. This expects the value to start at the lower bound, and approach the upper bound</param>
  /// <remarks>
  /// It is likely easier to export the curve and modify it in the editor to fine tune the value.
  /// You can use <c>[Export(PropertyHint.ExpEasing, "attenuation")]</c> on a float to see this curve in the inspector.
  /// You may omit the second property hint based on whether you want to attenuate the value or not.
  /// </remarks>
  /// <seealso href="https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_exports.html#floats-with-easing-hint">  Godot Docs: C# Export Attribute: ExpEasing</seealso>
  /// <br/>
  /// <seealso href="https://raw.githubusercontent.com/godotengine/godot-docs/master/img/ease_cheatsheet.png">  Easing curve cheatsheet</seealso>
  public static float RemapWithEase(float value, float inStart, float inStop, float outStart, float outStop, float easingCurve = 1, bool attenuate = false) {
    const float N_IN = 0;
    const float N_OUT = 1;

    float nIn = attenuate ? N_OUT : N_IN;
    float nOut = attenuate ? N_IN : N_OUT;
    
    value = Mathf.Remap(value, inStart, inStop, nIn, nOut);
    value = Mathf.Ease(value, easingCurve);
    value = Mathf.Remap(value, nIn, nOut, outStart, outStop);
    
    return value;
  }
}
