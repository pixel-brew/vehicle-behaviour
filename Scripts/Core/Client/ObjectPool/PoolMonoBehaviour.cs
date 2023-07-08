using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Client
{
    public class PoolMonoBehaviour : MonoBehaviour, IPoolable
    {
        [SerializeField] private bool _isAutoDestroyable;
        [SerializeField] private float _timeTillDestroy = 3f;

        private GameObjectPool<PoolMonoBehaviour> _origin;

        private IDisposable _destroySchedule;

        public void Setup(GameObjectPool<PoolMonoBehaviour> origin)
        {
            _origin = origin;
            if (_isAutoDestroyable)
            {
                ProcessDestroy(_timeTillDestroy);
            }
        }

        void IPoolable.Enable()
        {
        }

        private async void ProcessDestroy(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), ignoreTimeScale: false);
            _origin.Release(this);
        }

        void IPoolable.Disable()
        {
            gameObject.SetActive(false);
        }

        void IPoolable.Destroy()
        {
        }
    }
}