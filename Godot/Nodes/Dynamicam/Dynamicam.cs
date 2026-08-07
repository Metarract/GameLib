/* NOTE
 * can't be imported normally, has to be a child of the project
 */

using Godot;
using Metarract.GameLib.Godot.Nodes.Dynamicam.Strategies;

namespace Metarract.GameLib.Godot.Nodes.Dynamicam;
[GlobalClass]
public partial class Dynamicam : Camera2D {
  public static readonly Vector2 ACTOR_CAM_OFFSET = new(8, 8);

  [Export] public double ZoomDuration { get; set; } = 1;
  // [Export] public DynamicamStrategy CameraStrategy { get; private set; }
  private DynamicamStrategy CameraStrategy { get; set; }
  private Tween _zoomScaleTween;

  public override void _Process(double delta) {
    CameraStrategy?.Process(delta);
  }

  public void SetZoomScale(float zoom) {
    _zoomScaleTween?.Kill();
    _zoomScaleTween = CreateTween()
      .SetEase(Tween.EaseType.Out)
      .SetTrans(Tween.TransitionType.Quad);

    _zoomScaleTween.TweenProperty(this, "zoom", new Vector2(zoom, zoom), ZoomDuration);
  }

  public void SetCameraStrategy(DynamicamStrategy cameraStrategy) {
    CameraStrategy?.OnExit();
    CameraStrategy = cameraStrategy;
    CameraStrategy.OnEnter();
  }
}
