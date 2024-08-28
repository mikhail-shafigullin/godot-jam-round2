using Godot;
using Godot.Collections;

namespace GodotJamRound2.gameplay;

public partial class MissionManager : Resource
{
    
    private MissionRes _currentMissionRes;
    private Array<MissionRes> previousMissions = new Array<MissionRes>();
    
    public MissionRes GetCurrentMission()
    {
        return _currentMissionRes;
    }
    
    
    public bool IsMissionComplete()
    {
        return _currentMissionRes.IsComplete();
    }
    
    public void OnTaskComplete()
    {
        EmitSignal(nameof(OnCurrentMissionChanged));
        if(IsMissionComplete())
        {
            previousMissions.Add(_currentMissionRes);
        }
    }
    
    [Signal]
    public delegate void OnCurrentMissionChangedEventHandler();


    public void StartMission(MissionRes missionRes)
    {
        _currentMissionRes = missionRes;
        missionRes.SetMissionManager(this);
        EmitSignal(nameof(OnCurrentMissionChanged));
    }
}