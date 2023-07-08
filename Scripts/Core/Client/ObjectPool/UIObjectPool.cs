using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Client
{
    [Serializable]
    public class UiObjectsPool<T> where T : MonoBehaviour, IPoolable
    {
        private GameObjectPool<T> _gameObjectPool;
        
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _parent;
        [SerializeField] private int _capacity;

        private List<T> _acquiredElems = new List<T>();

        void OnAwake()
        {
            _gameObjectPool = new GameObjectPool<T>(_prefab, _parent, _capacity);
        }
        
        public T Acquire()
        {
            var result = _gameObjectPool.Acquire();
            result.transform.SetSiblingIndex(_acquiredElems.Count);
            _acquiredElems.Add(result);
            return result;
        }

        public void Release(T obj)
        {
            var index = _acquiredElems.IndexOf(obj);

            if (index != -1)
            {
                _gameObjectPool.Release(obj);
                _acquiredElems.Remove(obj);
            }
            else
            {
                Debug.LogError("object was not spawned through pool");
            }
        }

        public void ReleaseAllAcquired()
        {
            foreach (var elem in _acquiredElems)
            {
                _gameObjectPool.Release(elem);
            }   
            
            _acquiredElems.Clear();
        }
    }
}