using Core.Client.Context;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Client.Battle.Aircraft
{
    public class AircraftModule : IClientContextModule
    {
        private Aircraft _aircraft;
        
        public IAircraft Aircraft => _aircraft;
      
        public virtual void Initialize(IClientContext context)
        {
        }

        public virtual async UniTask Load()
        {
            _aircraft = CreateAircraft();
            await UniTask.Yield();
        }

        private Aircraft CreateAircraft()
        {
            var aircraftObject = new GameObject("aircraft");
            var aircraft = aircraftObject.AddComponent<Aircraft>();
            aircraft.Setup();
            return aircraft;
        }

        public virtual async UniTask Unload()
        {
            await UniTask.Yield();
        }
    }
}