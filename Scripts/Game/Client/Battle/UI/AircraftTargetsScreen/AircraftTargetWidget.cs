using Core.Client;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

namespace Game.Client.Battle.UI
{
    public class AircraftTargetWidget : MonoBehaviour, IPoolable
    {
        [Space]
        [SerializeField] private SerializableDictionaryBase<AircraftTargetType, AircraftVisualWidget> _visualWidgets;

        private AircraftVisualWidget _currentVisual;
        private IAircraftTarget _target;

        public void SetVisualWidget(IAircraftTarget target)
        {
            Disable();

            _target = target;
            _currentVisual = _visualWidgets[target.AircraftTargetType];
        }
        
        public  void Enable()
        {
            _currentVisual.EnableVisual(_target);
        }
        
        public void Disable()
        {
            foreach (var visual in _visualWidgets.Values)
            {
                visual.DisableVisual();
            }
        }

        void IPoolable.Destroy()
        {
            Destroy(gameObject);
        }
    }   
}