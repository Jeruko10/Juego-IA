using Godot;
using System.Collections.Generic;

namespace Game;

public partial class VirtualInputProvider : Node2D, IInputProvider
{
    readonly Queue<Vector2I> queuedLeftClicks = new();
    readonly Queue<Vector2I> queuedRightClicks = new();
    bool passTurnNextFrame = false;
    Vector2I? hoveredCell = null;

    public void SimulateLeftClick(Vector2I cell) => queuedLeftClicks.Enqueue(cell);
    public void SimulateRightClick(Vector2I cell) => queuedRightClicks.Enqueue(cell);
    public void SimulateHover(Vector2I? cell) => hoveredCell = cell;
    public void SimulatePassTurn() => passTurnNextFrame = true;

    public Vector2I? GetLeftClickedCell()
    {
        throw new System.NotImplementedException();
    }

    public Vector2I? GetRightClickedCell()
    {
        throw new System.NotImplementedException();
    }

    public Vector2I? GetHoveredCell()
    {
        throw new System.NotImplementedException();
    }

    public bool IsTurnPassPressed()
    {
        throw new System.NotImplementedException();
    }
}
