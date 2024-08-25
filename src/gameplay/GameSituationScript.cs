using System;
using Godot;
using Godot.Collections;

namespace GodotJamRound2.gameplay;

public partial class GameSituationScript: Resource
{
    Array<GameSituation> _gameSituations;
    
    public GameSituationScript()
    {
        _gameSituations = new Array<GameSituation>();
    }
    
    public void AddGameSituation(GameSituation gameSituation)
    {
        _gameSituations.Add(gameSituation);
    }
}