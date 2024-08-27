using Godot;
using System;

public partial class SpaceshipLevel : Node3D
{
	private Camera3D Camera3D;
	private DronPlayer Player;
	
	public override void _Input(InputEvent @event)
	{
		// if input 'change_camera' is pressed
		if (@event.IsActionPressed("change_camera"))
		{
			Player.Camera3D.Current = !Player.Camera3D.Current;
			Camera3D.Current = !Camera3D.Current;
			GD.Print("12312", Camera3D.Current, Player.Camera3D.Current);
			
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Camera3D = GetNode<Camera3D>("%Camera3D");
		Player = GetNode<DronPlayer>("%DronPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
