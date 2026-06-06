using Godot;

namespace Metarract.GameLib.Godot.Nodes.Dynamicam.Strategies;
/// <summary>
/// Matches the viewport
/// </summary>
/// <param name="camera"></param>
public partial class DynamicamBasic(Dynamicam camera) : DynamicamStrategy(camera) {
  private Tween _zoomOutTween;

  public override void OnEnter() {
    _zoomOutTween = Camera.CreateTween()
      .SetEase(Tween.EaseType.Out)
      .SetTrans(Tween.TransitionType.Back);
    
    // TODO set with respect to the window size
    _zoomOutTween.TweenProperty(Camera, "global_position", Camera.GlobalPosition, 1);
    
    Camera.SetZoomScale(1);
  }
}
