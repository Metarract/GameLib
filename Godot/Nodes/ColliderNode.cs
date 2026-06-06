using Godot;
using Metarract.GameLib.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Metarract.GameLib.Godot.Nodes;

public abstract partial class ColliderNode<TP> : Area2D where TP : Node2D {
  private TP ParentNode;
  private List<string> Warnings = [];

  public override void _Ready() {
    // if (Owner is TP parent) {
    //   ParentNode = parent;
    // } else {
    //   Warnings.Add($"Parent must be of type '{typeof(TP).FullName}', was: '{Owner.GetType().FullName}'");
    //   if (Engine.IsEditorHint()) {
    //     // see below, _GetConfigurationWarnings()
    //     // UpdateConfigurationWarnings();
    //   } else {
    //     throw new CustomNodeConfigurationException(Warnings.ToArray());
    //   }
    // }
  }

  // doesn't seem to work currently, most likely due to how Godot handles assigning scripts to objects in the
  // editor vs at runtime
  // public override string[] _GetConfigurationWarnings() => Warnings is not null ? Warnings.ToArray() : base._GetConfigurationWarnings();

  /// <summary>
  /// Wrap this with a method on the concrete implementation that defines your proper return type.
  /// </summary>
  /// <typeparam name="TC">The other concrete ColliderNode to check for</typeparam>
  /// <typeparam name="T">The ColliderNode's ParentNode type</typeparam>
  protected List<T> GetOverlappingTargets<TC, T>() where T : Node2D where TC : ColliderNode<T> {
    var cs = GetOverlappingAreas().OfType<TC>().ToList();
    return cs.Select(c => c.ParentNode).ToList();
  }
}
