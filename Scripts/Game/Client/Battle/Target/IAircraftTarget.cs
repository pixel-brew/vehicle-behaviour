using UnityEngine;

namespace Game.Client.Battle
{
    public interface IAircraftTarget 
    {
        Bounds Bounds { get; }
        Vector3 Position { get; }
        Vector2 DisplayOffset { get; }
        AircraftTargetType AircraftTargetType { get; }
    }   
}