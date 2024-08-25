using Godot;
using System;

public partial class Globals : Node
{
	private BasePlayerUI _ui;
	private GameController _gameController;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void SetPlayerUI(BasePlayerUI ui)
	{
		_ui = ui;
	}
	
	public BasePlayerUI GetPlayerUI()
	{
		return _ui;
	}
	
	public void SetGameController(GameController gameController)
	{
		_gameController = gameController;
	}
	
	public GameController GetGameController()
	{
		return _gameController;
	}
	
}
