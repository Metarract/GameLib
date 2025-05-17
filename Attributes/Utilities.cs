using Godot;
using System.Reflection;

namespace Metarract.Attributes;
public static class Utilities {
  /// <summary>
  /// Simple / Clean method of getting child nodes for props/fields that have the <c>[Node]</c> Attribute.
  /// This should be used in the <c>_Ready()</c> method.
  /// <seealso cref="NodeAttribute"/>
  /// </summary>
  public static void GetNodes(this Node node) {
    var t = node.GetType();
    var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    var properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    foreach (FieldInfo fieldInfo in fields) {
      var att = (NodeAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(NodeAttribute));
      if (att is null) continue;
      string nameRef = NodeAttribute.PREFIX + fieldInfo.Name;
      Node nodeRef = node.GetNode(nameRef);
      fieldInfo.SetValue(node, nodeRef);
    }

    foreach (PropertyInfo propertyInfo in properties) {
      var att = (NodeAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(NodeAttribute));
      if (att is null) continue;
      var nodeRef = GetNodeByMemberInfo(node, propertyInfo);
      propertyInfo.SetValue(node, nodeRef);
    }
  }

  private static Node GetNodeByMemberInfo(Node node, MemberInfo memberInfo) =>
    node.GetNode(NodeAttribute.PREFIX + memberInfo.Name);
}
