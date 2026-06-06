using Godot;
using System.Collections.Generic;

namespace Metarract.GameLib.Godot;
public static class Drawing {
  public static void AddLine2D(this Node parent, List<Vector2> points, string color = "red", float width = 1) {
    var line = new Line2D {
      DefaultColor = new Color(color),
      Points = [.. points],
      Width = width
    };
    parent.AddChild(line);
  }

  public static void DrawCrosshair(Node parent, Vector2 position, float length = 5, string color = "red",
    float width = 1) {
    List<Vector2> points = [
      position + Vector2.Left * length,
      position + Vector2.Right * length,
      position,
      position + Vector2.Up * length,
      position + Vector2.Down * length
    ];
    AddLine2D(parent, points, color, width);
  }
}
