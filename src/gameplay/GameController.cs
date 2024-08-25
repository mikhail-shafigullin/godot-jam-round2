using Godot;
using System;
using Godot.Collections;
using GodotJamRound2.gameplay;
using GodotJamRound2.mechas;
using Array = System.Array;

public partial class GameController : Node
{
	[Export]
	private Array<MechaPanel> _mechaPanels = new Array<MechaPanel>();
	
	private Globals _globals = null;
	
	private HangarRes _hangarRes;
	
	private Array<GameSituationScript> _gameSituationScripts = new Array<GameSituationScript>();
	
	private Timer _timer;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_globals = GetNode<Globals>("/root/Globals");
		_globals.SetGameController(this);

		_hangarRes = new HangarRes();
		
		if(_mechaPanels.Count != 3){
			GD.PrintErr("There should be 3 mecha panels");
		}
		
		_mechaPanels[0].SetHangarRes(_hangarRes.GetPanel(0));
		_mechaPanels[1].SetHangarRes(_hangarRes.GetPanel(1));
		_mechaPanels[2].SetHangarRes(_hangarRes.GetPanel(2));
		
		_timer = new Timer();
		_timer.OneShot = true;
		AddChild(_timer);

		CreateScripts();
		
		RunGameSituationScripts(_gameSituationScripts[0]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void CreateScripts()
	{
		_gameSituationScripts = new Array<GameSituationScript>();
		var firstScript = new GameSituationScript();
		_gameSituationScripts.Add(firstScript);
		firstScript.AddGameSituation(new GameSituation(_hangarRes.GetPanel(0).createMechaSignal, 2));
		firstScript.AddGameSituation(new GameSituation(_hangarRes.GetPanel(1).createMechaSignal, 3));
		firstScript.AddGameSituation(new GameSituation(_hangarRes.GetPanel(2).createMechaSignal, 3));
		
		var secondScript = new GameSituationScript();
		_gameSituationScripts.Add(secondScript);
		
		var thirdScript = new GameSituationScript();
		_gameSituationScripts.Add(thirdScript);
	}

	private void RunGameSituationScripts(GameSituationScript gameSituationScript)
	{
		gameSituationScript.Run(_timer);
	}
	
	
}
