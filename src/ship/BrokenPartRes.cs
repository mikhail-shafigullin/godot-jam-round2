using System;
using Godot;

namespace GodotJamRound2.ship;

public partial class BrokenPartRes: Resource
{
    private Vector3 _position;
    private float repairProgress = 0;
    public static float maxRepairProgress = 100;
    private bool isRepaired = false;
    private bool isDisabled = false;
    
    public void SetPosition(Vector3 position)
    {
        _position = position;
    }
    
    public Vector3 GetPosition()
    {
        return _position;
    }
    
    public void SetRepairProgress(float progress)
    {
        if (!isRepaired)
        {
            repairProgress = Mathf.Clamp(progress, 0, maxRepairProgress);
            EmitSignal(nameof(OnRepairChanged));
            if(repairProgress >= maxRepairProgress)
            {
                isRepaired = true;
                EmitSignal(nameof(OnPartRepaired));
            }
        }
    }
    
    public void SetDisabled(bool disabled)
    {
        isDisabled = disabled;
        EmitSignal(nameof(OnPartDisabled));
    }
    
    [Signal]
    public delegate void OnPartRepairedEventHandler();
    
    [Signal]
    public delegate void OnRepairChangedEventHandler();
    
    [Signal]
    public delegate void OnPartDisabledEventHandler();
}