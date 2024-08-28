using Godot;
using System;

public partial class MapUiController : Control
{
	private Globals _globals = null;
	
	private TextureRect leftTextureRect;
	private TextureRect topTextureRect;

	private Control zBackLeftMarker;
	private Control zForwardLeftMarker;
	private Control yTopLeftMarker;
	private Control yBottomLeftMarker;
	
	private Control zBackTopMarker;
	private Control zForwardTopMarker;
	private Control xLeftTopMarker;
	private Control xRightTopMarker;

	private Control LeftPlayerMarker;
	private Control TopPlayerMarker;
	
	private GameController _gameController;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_globals = GetNode<Globals>("/root/Globals");
		
		zBackLeftMarker = GetNode<Control>("%zBackLeft");
		zForwardLeftMarker = GetNode<Control>("%zForwardLeft");
		yTopLeftMarker = GetNode<Control>("%yTopLeft");
		yBottomLeftMarker = GetNode<Control>("%yBottomLeft");
		
		zBackTopMarker = GetNode<Control>("%zBackTop");
		zForwardTopMarker = GetNode<Control>("%zForwardTop");
		xLeftTopMarker = GetNode<Control>("%xLeftTop");
		xRightTopMarker = GetNode<Control>("%xRightTop");
		
		LeftPlayerMarker = GetNode<Control>("%LeftPlayerMarker");
		TopPlayerMarker = GetNode<Control>("%TopPlayerMarker");

		LeftPlayerMarker.Visible = true;
		TopPlayerMarker.Visible = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		WritePlayerPosition();
	}

	public void WritePlayerPosition()
	{
		_gameController = _globals.GetGameController();
		Vector3 playerPositionForMap = _gameController.GetPlayerPositionForMap();

		Vector2 playerPositionOnLeftMap = Vector2.Zero;
		playerPositionOnLeftMap.X = zBackLeftMarker.GlobalPosition.X + (zForwardLeftMarker.GlobalPosition.X - zBackLeftMarker.GlobalPosition.X) * playerPositionForMap.Z;
		playerPositionOnLeftMap.Y = yBottomLeftMarker.GlobalPosition.Y + (yTopLeftMarker.GlobalPosition.Y - yBottomLeftMarker.GlobalPosition.Y) * playerPositionForMap.Y;
		
		Vector2 playerPositionOnTopMap = Vector2.Zero;
		playerPositionOnTopMap.X = zBackTopMarker.GlobalPosition.X + (zForwardTopMarker.GlobalPosition.X - zBackTopMarker.GlobalPosition.X) * playerPositionForMap.Z;
		playerPositionOnTopMap.Y = xLeftTopMarker.GlobalPosition.Y + (xRightTopMarker.GlobalPosition.Y - xLeftTopMarker.GlobalPosition.Y) * playerPositionForMap.X;
		
		GD.Print(playerPositionOnLeftMap, playerPositionOnTopMap);
		LeftPlayerMarker.GlobalPosition = playerPositionOnLeftMap;
		TopPlayerMarker.GlobalPosition = playerPositionOnTopMap;
	}
}
