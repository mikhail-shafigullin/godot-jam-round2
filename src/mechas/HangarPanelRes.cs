using Godot;

namespace GodotJamRound2.mechas;

public partial class HangarPanelRes: Resource
{
    private int _index;
    private MechaRes _mechaRes = null;
    public Signal createMechaSignal;
    public Signal launchMechaSignal;
    
    public HangarPanelRes()
    {
        createMechaSignal = new Signal(this, "OnCreateMecha");
        launchMechaSignal = new Signal(this, "OnLaunchMecha");
    }
    
    public void SetMecha(MechaRes mechaRes)
    {
        _mechaRes = mechaRes;
    }
    
    public MechaRes GetMecha()
    {
        return _mechaRes;
    }
    
    public void SetIndex(int index)
    {
        _index = index;
    }

    [Signal]
    public delegate void OnCreateMechaEventHandler();
    
    [Signal]
    public delegate void OnLaunchMechaEventHandler();

    public override string ToString()
    {
        return "Hangar Panel #" + _index;
    }
    
    public void CreateMecha()
    {
        EmitSignal(createMechaSignal.Name);
    }
}