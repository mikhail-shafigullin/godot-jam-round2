using Godot;
using Godot.Collections;

namespace GodotJamRound2.mechas;

public partial class HangarRes: Resource
{
    private Array<HangarPanelRes> _hangarPanelRes = new Array<HangarPanelRes>();
    
    public HangarRes()
    {
        _hangarPanelRes.Add(new HangarPanelRes());
        _hangarPanelRes.Add(new HangarPanelRes());
        _hangarPanelRes.Add(new HangarPanelRes());
        
        _hangarPanelRes[0].SetIndex(0);
        _hangarPanelRes[1].SetIndex(1);
        _hangarPanelRes[2].SetIndex(2);
        
        
        if(_hangarPanelRes.Count != 3){
            GD.PrintErr("There should be 3 mecha panels");
        }
    }
    
    public HangarPanelRes GetPanel(int index)
    {
        return _hangarPanelRes[index];
    }
}