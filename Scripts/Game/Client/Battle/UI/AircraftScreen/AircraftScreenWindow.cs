using System.Collections.Generic;
using System.Linq;
using Core.Client.UI;
using Game.Client.Battle.Aircraft;
using UnityEngine;

namespace Game.Client.Battle.UI
{
    public class AircraftScreenWindow : Window
    {
        [Space] 
        [SerializeField] private AircraftControlWidget _aircraftControlWidget;
        [SerializeField] private OverheatingIndicator _overheatingIndicator;
        [SerializeField] private WeaponButton _firstWeaponButton;
        [SerializeField] private WeaponButton _secondWeaponButton;
        [SerializeField] private WeaponButton _thirdWeaponButton;
        [SerializeField] private List<AimBehaviour> _aimBehaviours;

        private AimBehaviour _currentAim;

        protected override void OnSetup()
        {
            base.OnSetup();
            _currentAim = _aimBehaviours.First();
            SetupButtons();
            var aircraftModule = Context.GetModule<AircraftModule>();
            // _aircraftControlWidget.Setup(aircraftModule.Aircraft); //TODO: 
        }

        protected override void OnShow()
        {
            base.OnShow();
        }
        
        private void SetupButtons()
        {
            _firstWeaponButton.EnableButton();
            _secondWeaponButton.DisableButton();
            _thirdWeaponButton.DisableButton();
        }
        
        private void OnShootWeapon(int idWeapon)
        {
            if (_overheatingIndicator.IsOverhetead)
            {
                _currentAim.ResetSpread();
                return;
            }
            
            if (_currentAim != _aimBehaviours[idWeapon])
            {
                ChangeAim(idWeapon);
            }
            
            // _overheatingIndicator.SetHeating(0.025f, _firstWeaponButton.HoldDuration);
            _currentAim.SetSpread(1f);
        }

        private void ChangeAim(int idWeapon)
        {
            _currentAim.DisableAim();
            _currentAim = _aimBehaviours[idWeapon];
            _currentAim.EnableAim();
        }
    }
}