using Godot;
using System;

namespace Game;

public partial class TileDisplay : Node2D
{
	[ExportSubgroup("References")]
	[Export] public Sprite2D Sprite { get; private set; }
	
}
