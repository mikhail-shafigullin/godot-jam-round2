using System;
using Godot;
using GodotJamRound2.mechas;

namespace GodotJamRound2.ship;

public partial class BrokenPartRes: Resource
{
    private Vector3 _position;
    private float repairProgress = 0;
    public static float maxRepairProgress = 100;
    private bool isRepaired = false;
    
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
    
    [Signal]
    public delegate void OnPartRepairedEventHandler();
    
    [Signal]
    public delegate void OnRepairChangedEventHandler();
}