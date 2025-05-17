namespace Metarract.Attributes;
/// <summary>
/// To be used with the <see cref="Utilities.GetNodes" /> extension method on properties and fields.
/// Class members should either have the same name as the Node in the Godot Editor as well as having a Unique Name set,
/// or the attribute should specify a <c>NodePath</c>.
/// </summary>
/// <example>
/// <code>
/// // %CharacterSprite
/// [Node] private Sprite2D CharacterSprite { get; set; }
/// // 
/// [Node(NodePath = "/UI/VBoxContainer/NameLabel")]
/// public Label CharacterNameLabel;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class NodeAttribute : Attribute {
  public const char PREFIX = '%';
  public string NodePath { get; set; }
}
