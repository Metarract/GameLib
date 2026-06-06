using Godot;

namespace Metarract.GameLib.Godot.Nodes.Dynamicam.Strategies;
[GlobalClass]
public partial class DynamicamFollow(Node2D target, Dynamicam camera) : DynamicamStrategy(camera) {
  private bool _nearby;
  private Tween _followTween;

  public override void OnEnter() {
    _followTween = Camera.CreateTween()
      .SetEase(Tween.EaseType.Out)
      .SetTrans(Tween.TransitionType.Back);
    _followTween.TweenProperty(Camera, "global_position", target.GlobalPosition + Dynamicam.ACTOR_CAM_OFFSET, 1);
    _followTween.TweenCallback(Callable.From(SetNearby));

    Camera.SetZoomScale(3);
  }

  public override void Process(double delta) {
    if (!_nearby) return;
    Camera.GlobalPosition = target.GlobalPosition + Dynamicam.ACTOR_CAM_OFFSET;
  }

  public override void OnExit() => _followTween?.Kill();

  private void SetNearby() => _nearby = true;
}
