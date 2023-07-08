using UnityEngine;
using UnityEngine.UI;

namespace Game.Client.Battle.UI
{
    public class AircraftVisualWidget : MonoBehaviour
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private Image _healthBar;

        private float _scaleFactor = 50f;

        public void EnableVisual(IAircraftTarget target)
        {
            gameObject.SetActive(true);
            UpdateScale(target);
            SetHealth(target); //TODO: add health data in target
        }

        public void DisableVisual()
        {
            gameObject.SetActive(false);
        }

        private void UpdateScale(IAircraftTarget target)
        {
            if (!_container)
            {
                return;
            }
            
            _container.sizeDelta = target.Bounds.size * _scaleFactor;
        }

        private void SetHealth(IAircraftTarget target)
        {
            if (!_healthBar)
            {
                return;
            }
            
            //TODO: get health data
        }
    }
}