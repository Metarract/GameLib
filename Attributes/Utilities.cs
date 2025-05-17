using Godot;
using System.Reflection;

namespace Metarract.Attributes;
public static class Utilities {
  /// <summary>
  /// Simple / clean method of setting Nodes for props/fields that have the <see cref="NodeAttribute"/>.
  /// Should be used in the <c>_Ready()</c> method.
  /// </summary>
  /// <example>
  /// <code>
  /// public override void _Ready() {
  ///   this.GetNodes();
  /// }
  /// </code>
  /// </example>
  public static void GetNodes(this Node node) {
    Type t = node.GetType();
    FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    PropertyInfo[] properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    foreach (FieldInfo fieldInfo in fields) {
      var att = (NodeAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(NodeAttribute));
      if (att is null) continue;
      string nodePath = GetNodePath(att, fieldInfo);
      Node nodeRef = node.GetNode(nodePath);
      fieldInfo.SetValue(node, nodeRef);
    }

    foreach (PropertyInfo propertyInfo in properties) {
      var att = (NodeAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(NodeAttribute));
      if (att is null) continue;
      string nodePath = GetNodePath(att, propertyInfo);
      Node nodeRef = node.GetNode(nodePath);
      propertyInfo.SetValue(node, nodePath);
    }
  }

  private static string GetNodePath(NodeAttribute att, MemberInfo memberInfo) => 
    att.NodePath ?? NodeAttribute.PREFIX + memberInfo.Name;
}
