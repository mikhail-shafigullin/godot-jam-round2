using Godot;

namespace GodotJamRound2.gameplay;

public partial class MissionManager : Resource
{
    
    private MissionRes _currentMissionRes;
    
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
        if(IsMissionComplete())
        {
            GD.Print("Mission Complete");
        }
    }
    
    [Signal]
    public delegate void OnCurrentMissionChangedEventHandler();


    public void StartMission(MissionRes missionRes)
    {
        _currentMissionRes = missionRes;
        EmitSignal(nameof(OnCurrentMissionChanged));
    }
}