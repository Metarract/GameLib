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
    foreach (FieldInfo f in fields) SetNodeByAttribute(node, f);
    foreach (PropertyInfo p in properties) SetNodeByAttribute(node, p);
  }

  private static void SetNodeByAttribute<T>(Node node, T m) where T : MemberInfo {
    var att = (NodeAttribute)Attribute.GetCustomAttribute(m, typeof(NodeAttribute));
    if (att is null) return;
    string nodePath = att.NodePath ?? NodeAttribute.PREFIX + m.Name;
    Node nodeRef = node.GetNode(nodePath);
    switch (m) {
      case FieldInfo f:
        f.SetValue(node, nodeRef);
        break;
      case PropertyInfo p:
        p.SetValue(node, nodeRef);
        break;
    }
  }
}
