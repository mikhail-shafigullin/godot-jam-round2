using System;
using Godot;
using Godot.Collections;

namespace GodotJamRound2.ship;

public partial class ShipRes: Resource
{
    private Dictionary<String, Array<BrokenPartRes>> _brokenPartGroups = new Dictionary<String, Array<BrokenPartRes>>();
    
    public ShipRes()
    {
        _brokenPartGroups = new Dictionary<String, Array<BrokenPartRes>>();
        
        var brokenParts1 = new Array<BrokenPartRes>();
        brokenParts1.Add(new BrokenPartRes());
        brokenParts1.Add(new BrokenPartRes());
        _brokenPartGroups.Add("firstMission", brokenParts1);
        
        var brokenParts2 = new Array<BrokenPartRes>();
        brokenParts2.Add(new BrokenPartRes());
        brokenParts2.Add(new BrokenPartRes());
        brokenParts2.Add(new BrokenPartRes());
        _brokenPartGroups.Add("secondMission", brokenParts2);
    }
    
    public Array<BrokenPartRes> GetBrokenParts(String group)
    {
        return _brokenPartGroups[group];
    }
    
    
}