using System;
using Game.Client.Battle.Aircraft;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.Client.Battle.UI
{
    public class WeaponButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private TextMeshProUGUI _weaponName;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private OverheatingIndicator _overheatingIndicator;
        
        private float _disableAlpha = 0.5f;
        private float _enableAlpha = 1f;

        private IAircraft _aircraft;
        
        public void Setup(IAircraft aircraft, int weaponIndex)
        {
            _aircraft = aircraft;
        }
        
        public void EnableButton()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = _enableAlpha;
        }
        
        public void DisableButton()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = _disableAlpha;
        }

        public void SetWeaponName(string weaponName)
        {
            _weaponName.text = weaponName;
        }

        public void SetWeaponLevel(int level)
        {
            
        }

        public void SetWeaponWorkingPrincipleType()
        {
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            
        }
    }
}