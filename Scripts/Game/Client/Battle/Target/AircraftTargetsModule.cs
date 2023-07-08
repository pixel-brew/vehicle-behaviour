using System;
using System.Collections.Generic;
using Core.Client.Context;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Game.Client.Battle
{
    public class AircraftTargetsModule : IClientContextModule
    {
        private List<IAircraftTarget> _targets = new List<IAircraftTarget>();
        public List<IAircraftTarget> Targets => _targets;

        public event Action<AircraftTarget> OnTargetCreated;
        public event Action<AircraftTarget> OnTargetDestroyed;
        
        void IClientContextModule.Initialize(IClientContext context)
        {
            
        }

        async UniTask IClientContextModule.Load()
        {
            GetTargetsTemporary();
        }

        async UniTask IClientContextModule.Unload()
        {
            
        }
        
        private void GetTargetsTemporary()
        {
            // temporary
            var objects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var gameObject in objects)
            {
                var targets = gameObject.GetComponentsInChildren<AircraftTarget>();
                if (targets.Length > 0)
                {
                    _targets.AddRange(targets);
                }
            }
        }
    }
}