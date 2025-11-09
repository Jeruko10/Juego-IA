using Godot;

namespace Game;

[GlobalClass]
public partial class MouseInputProvider : Node2D, IInputProvider
{
    public Vector2I? GetHoveredCell()
    {
        throw new System.NotImplementedException();
    }

    public Vector2I? GetLeftClickedCell()
    {
        throw new System.NotImplementedException();
    }

    public Vector2I? GetRightClickedCell()
    {
        throw new System.NotImplementedException();
    }

    public bool IsTurnPassPressed()
    {
        throw new System.NotImplementedException();
    }
}
