namespace Metarract.Attributes;
/// <summary>
/// To be used with the <c>GetNodes</c> Node extension method.
/// Node members should have the custom attribute <c>[Node]</c>, and the same name as the Node in the Godot Editor,
/// as well as having a Unique Name set. e.g.: <c>"%NodeNameHere"</c>
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class NodeAttribute : Attribute {
  public const char PREFIX = '%';
}
