using Godot;
using GodotJamRound2.entites.ui;

namespace GodotJamRound2.mechas;

public partial class MechaPartRes: Resource
{
    private EMechaPartType _type = EMechaPartType.RIGHT_ARM;
    private float hp = 0.0f;
    private float maxHp = 100.0f;
    
    public void RepairWithEquipmentWithDelta(PlayerEquipmentRes equipmentRes, float delta)
    {
        if (hp < maxHp)
        {
            hp += equipmentRes.GetRepairSpeed() * delta;
            if (hp > maxHp)
            {
                hp = maxHp;
            }
        }
    }
    
    public void SetMechaPartType(EMechaPartType type)
    {
        _type = type;
    }
    
}