using System.Collections.Generic;
using UnityEngine;

namespace Game.Client.Battle.Aircraft
{
    public class Aircraft: MonoBehaviour, IAircraft
    {
        private Camera _camera;
        private readonly List<AircraftWeapon> _weapons = new List<AircraftWeapon>();

        public void Setup()
        {
            _camera = Camera.main;
        }

        void IAircraftControl.ZoomView(float deltaZoom)
        {
         
        }

        void IAircraftControl.MoveView(Vector2 delta)
        {
            
        }

        void IAircraftControl.ActivateWeaponTrigger(int weaponIndex)
        {
            
        }

        void IAircraftControl.DeactivateWeaponTrigger(int weaponIndex)
        {
            
        }

        int IAircraftStatus.WeaponsCount => _weapons.Count;

        IAircraftWeaponStatus IAircraftStatus.GetWeaponStatus(int weaponIndex)
        {
            return _weapons[weaponIndex];
        }
    }
}