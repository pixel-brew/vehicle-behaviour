
using UnityEngine;

namespace Game.Client.Battle.Aircraft
{
    public interface IAircraftControl
    {
        void ZoomView(float deltaZoom);
        void MoveView(Vector2 delta);
        void ActivateWeaponTrigger(int weaponIndex);
        void DeactivateWeaponTrigger(int weaponIndex);
    }
}