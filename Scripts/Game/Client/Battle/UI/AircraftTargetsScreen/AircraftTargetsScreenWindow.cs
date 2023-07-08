using System;
using System.Collections.Generic;
using Core;
using Core.Client;
using Core.Client.UI;
using UnityEngine;

namespace Game.Client.Battle.UI
{
    public class AircraftTargetsScreenWindow: Window
    {    
        [Serializable]
        private class TargetIndicatorPool : UiObjectsPool<AircraftTargetWidget>
        {
        }
        
        private Camera _camera;
        private Plane[] _frustum;

        private Dictionary<IAircraftTarget, AircraftTargetWidget> _targets;
        [SerializeField] private TargetIndicatorPool _targetIndicatorPool;
        [SerializeField] private AircraftScreenSettings _screenSettings;
        
        [SerializeField] private AircraftTargetWidget _prefab; //test while the pool is broken
        
        protected override void OnSetup()
        {
            base.OnSetup();
            _camera = Camera.main;
            _targets = new Dictionary<IAircraftTarget, AircraftTargetWidget>();

            Debug.LogError("uncomment");
            // var aircraftTargetsModule = Context.GetModule<AircraftTargetsModule>();
            // var targets = aircraftTargetsModule.Targets;
            // foreach (var target in targets)
            // {
            //     AddTarget(target);
            // }

            // aircraftTargetsModule.OnTargetDestroyed += RemoveTarget;
            // aircraftTargetsModule.OnTargetCreated += AddTarget;
        }

        private void AddTarget(IAircraftTarget target)
        {
            var widget = CreateTargetWidget();
            widget.SetVisualWidget(target);
            _targets.Add(target, widget);
        }

        private void RemoveTarget(IAircraftTarget target)
        {
            if (_targets.ContainsKey(target))
            {
                var widget = _targets[target];
                DestroyTargetWidget(widget);
                _targets.Remove(target);
            }
            else
            {
                Debug.LogError("aircraft target screen :: cannot find target to remove");
            }
        }
        
        private AircraftTargetWidget CreateTargetWidget()
        {
            Debug.LogError("instantiate");
            // return _targetIndicatorPool.Acquire(); //TODO:
            return Instantiate(_prefab, transform);
        }

        private void DestroyTargetWidget(AircraftTargetWidget targetWidget)
        {
            //_targetIndicatorPool.Release(targetWidget);
        }

        private void LateUpdate()
        {
            _frustum = GeometryUtility.CalculateFrustumPlanes(_camera);

            foreach (var item in _targets)
            {
                var target = item.Key;
                var widget = item.Value;

                UpdateWidgetPosition(target, widget);
            }
        }
        
        private void UpdateWidgetPosition(IAircraftTarget target, AircraftTargetWidget widget)
        {
            var bounds = target.Bounds;
            
            if (GeometryUtility.TestPlanesAABB(_frustum, bounds))
            {
                widget.Enable();
                widget.transform.position = _camera.WorldToScreenPoint(target.Position) + target.DisplayOffset.ToVector3();
            }
            else
            {
                widget.Disable();
            }
        }
    }
}