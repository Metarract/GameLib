using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Metarract.GameLib.Godot;
/// <summary>
/// SUPER WIP class for wrapping a 1d array, and performing 2d operations on it. Not ready!
/// </summary>
public class Grid<T>(int width, int height) : IEnumerable<T> {
  public int Width { get; } = width;
  public int Height { get; } = height;
  public Vector2I Size => new Vector2I(Width, Height);

  private readonly T[] _items = new T[width * height];

  public delegate void GridItemAction(T item, int x, int y);

  public delegate void GridPositionAction(int x, int y);

  // TODO - struct for relative checks? or something? see: RelativeGridCheck below
  public delegate void RelativeGridPositionAction(int relativeIndex, T item, Vector2I newPosition);

  #region indexing
#pragma warning disable CA2201
  // literally no reason to make a new exception that is just a copy of the IndexOutOfRangeException
  // why the fuck wouldn't i be allowed to throw an index out of range exception,
  // WHILE DEFINING AN INDEXER??
  public T this[int x, int y] {
    get {
      if (x < 0 || x >= Width || y < 0 || y >= Height) throw new IndexOutOfRangeException();
      return _items[y * Width + x];
    }
    set {
      if (x < 0 || x >= Width || y < 0 || y >= Height) throw new IndexOutOfRangeException();
      _items[y * Width + x] = value;
    }
  }

  public T this[Vector2I position] {
    get => this[position.X, position.Y];
    set => this[position.X, position.Y] = value;
  }
#pragma warning restore CA2201
  #endregion

  public enum ArrayDimension {
    WIDTH,
    HEIGHT,
  }

  public Grid(Vector2I dimensions) : this(dimensions.X, dimensions.Y) { }

  public int GetLength() => _items.Length;
  public int GetLength(int dimension) => GetLength((ArrayDimension)dimension);
  public int GetLength(ArrayDimension dimension) {
    return dimension switch {
      ArrayDimension.WIDTH => Width,
      ArrayDimension.HEIGHT => Height,
      _ => throw new ArgumentOutOfRangeException(nameof(dimension), dimension, "Buddy this was an enum for a reason, how the hell did you get here")
    };
  }

  #region Enumeration
  // public delegate void GridPositionHandler(int x, int y);
  // public static void ForGridPos<T>(this T[,] items, GridPositionHandler action) {
  //   for (int x = 0; x < items.GetLength(0); x++) {
  //     for (int y = 0; y < items.GetLength(1); y++) {
  //       action(x, y);
  //     }
  //   }
  // }
  
  public IEnumerator<T> GetEnumerator() {
    foreach (T item in _items) yield return item;
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  public void ForEach(Action<T> action) {
    foreach (T item in _items) action(item);
  }

  public void ForEach(GridPositionAction positionAction) {
    for (int x = 0; x < Width; x++) {
      for (int y = 0; y < Height; y++) {
        positionAction(x, y);
      }
    }
  }

  public void ForEach(GridItemAction itemAction) {
    for (int x = 0; x < Width; x++) {
      for (int y = 0; y < Height; y++) {
        itemAction(this[x, y], x, y);
      }
    }
  }
  #endregion

  public void Fill(T value) {
    ForEach((x, y) => {
      this[x, y] = value;
    });
  }

  public void Clear() {
    ForEach((x, y) => {
      this[x, y] = default;
    });
  }
}

// TODO: factory + builder for Relative Grid Check? 
public readonly struct RelativeGridCheck<T>(Grid<T> grid, Vector2I[] relativePositions, Func<T, bool> conditional) {
  /// <summary>
  /// Given a position, checks the established list of relative positions on this object and validates that the
  /// provided conditional returns true for all of them. If any fail, returns false.
  /// </summary>
  /// <param name="origin">Position to check from</param>
  /// <param name="failOnOutOfRange">
  /// Determines behaviour when the relative position is outside the bounds of the Grid.
  /// When true, will fail the entire check. When false, it will assume that the check succeeded and move on to the next
  /// position in the list.
  /// </param>
  public bool Check(Vector2I origin, bool failOnOutOfRange = true) {
    foreach (Vector2I relativePosition in relativePositions) {
      Vector2I checkPosition = origin + relativePosition;
      try {
        T item = grid[checkPosition];
        if (!conditional(item)) return false;
      } catch (IndexOutOfRangeException _) {
        if (failOnOutOfRange) return false;
      }
    }

    return true;
  }

  /// <summary>
  /// Performs a check on every spot on the provided Grid and returns a List of all Grid positions where the conditional
  /// passed for each position relative to it
  /// </summary>
  /// <param name="failOnOutOfRange">
  /// Determines behaviour when the relative position is outside the bounds of the Grid.
  /// When true, will fail the entire check. When false, it will assume that the check succeeded and move on to the next
  /// position in the list.
  /// </param>
  public List<Vector2I> GetValidPositions(bool failOnOutOfRange = true) {
    RelativeGridCheck<T> t = this;
    List<Vector2I> validPositions = [];
    
    grid.ForEach((x, y) => {
      Vector2I p = new(x, y);
      if (!t.Check(p, failOnOutOfRange)) return;
      validPositions.Add(p);
    });

    return validPositions;
  }
}

/* TODO:
 * consider this:
 * index into this as if it were [y,x]
 * WHY, you ask?
 * -
 * visualize with me, the 2 dimensional array
 * -
 * dimensions 3*3
 * elements 00, 01, 02,  10, 11, 12,  20, 21, 22
 * AKA,     00, 01, 02 / 10, 11, 12 / 20, 21, 22
 * 00 01 02
 * 10 11 12
 * 20 21 22
 * -
 * if listed out in our minds like normally, it's unintuitive to notice that for the purposes of our grid, this has
 * been ROTATED. x goes DOWN and y goes ACROSS visually
 * x
 * 0 [00 01 02],
 * 1 [10 11 12],
 * 2 [20 21 22],
 * y  0  1  2
 * -
 * in order to intuitively envision this, we'd have to display each array like so:
 * x    0    1    2
 * y 0 [00  [10  [20
 *   1  01   11   21
 *   2  02]  12]  22]
 * -
 * which is weird and certainly not the way you would envision the elements. so instead of doing this, just swap how
 * we index!
 * -
 * simpler? maybe! we shall see i suppose
 */
