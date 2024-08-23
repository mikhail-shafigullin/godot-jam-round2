using Godot;
using System;

public partial class Level1 : Node2D
{
	private ColorRect nonameHuisBlock;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		nonameHuisBlock = GetNode<ColorRect>("%SquareUI");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GD.Print("whatever " + nonameHuisBlock.Name);
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
			if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
				nonameHuisBlock.Visible = !nonameHuisBlock.Visible;
	}
	
}
