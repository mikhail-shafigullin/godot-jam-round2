using Godot;

namespace GodotJamRound2.mechas;

public partial class PlayerEquipmentRes: Resource
{
    private float _repairSpeed = 5.0f;
    
    public float GetRepairSpeed()
    {
        return _repairSpeed;
    }
}