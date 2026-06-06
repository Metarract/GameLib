using Godot;

namespace Metarract.GameLib.Godot.Nodes.Dynamicam.Strategies;
internal interface IDynamicamStrategy {
  void OnEnter() { }
  void OnExit() { }
  void Process(double delta) { }
}

public abstract partial class DynamicamStrategy(Dynamicam camera) : Resource, IDynamicamStrategy {
  public Dynamicam Camera { get; } = camera;
  public virtual void OnEnter() { }
  public virtual void OnExit() { }
  public virtual void Process(double delta) { }
}
