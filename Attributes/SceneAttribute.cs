using Godot;
using System;
using System.Reflection;

namespace Metarract.GameLib.Attributes;
// maybe some day..........

/// <summary>
/// Preloads a PackedScene of the object to instantiate, such that an instance can be created simply by
/// referencing the type
/// </summary>
/// <remarks>
/// DO NOT USE - Broken (at least as of: Godot 4.3) - This breaks C# scene -> script association,
/// somehow prevents Godot from seeing the class. Probably true of any class attribute
/// outside of expected ones
/// </remarks>
/// <example>
/// <code>
/// [Scene("res://ExampleScene.tscn")]
/// public partial class ExampleScene : Node2D
/// [...]
/// </code>
/// Usage:
/// <code>
/// SceneAttribute.GetInstance&lt;ExampleScene&gt;();
/// </code>
/// </example>
/// <param name="scenePath">Path to the scene file as provided by the Godot Editor</param>
[AttributeUsage(AttributeTargets.Class)]
public class SceneAttribute(string scenePath) : Attribute {
  // private readonly PackedScene PackedScene = GD.Load<PackedScene>(scenePath);
  public string ScenePath { get; private set; } = scenePath;

  // public static T GetInstance<T>() where T : Node {
  //   var att = (SceneAttribute)typeof(T).GetCustomAttribute(typeof(SceneAttribute));
  //   if (att is null) throw new MissingSceneAttributeException();
  //   return att.PackedScene.Instantiate<T>();
  // }
}

public class MissingSceneAttributeException : Exception { }


