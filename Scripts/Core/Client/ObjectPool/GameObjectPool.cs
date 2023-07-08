using System;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Core.Client
{
    public class GameObjectPool<T> : ObjectPool<T>, IDisposable where T : UnityEngine.MonoBehaviour, IPoolable
    {
        private readonly GameObject _prefab;
        private Transform _parent;
        
        public GameObjectPool(GameObject prefab, int capacity) : base(capacity)
        {
            if (prefab == null)
            {
                Debug.LogError("pool : prefab is null");
            }

            _prefab = prefab;
            
            Disabler = o =>
            {
                o.gameObject.SetActive(false);
                o.Disable();
            };
            Enabler = o =>
            {
                o.gameObject.SetActive(true);
                o.gameObject.transform.localPosition = _prefab.transform.localPosition;
                o.gameObject.transform.localRotation = _prefab.transform.localRotation;
                o.Enable();
            };
            Destoyer = o =>
            {
                o.Destroy();
                Object.Destroy(o.gameObject);
            };
            Constructor = () =>
            {
                var o = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity);
                if (o == null)
                {
                    Debug.LogError("pool : can not create object from prefab = " + _prefab);
                }

                var c = o.GetComponent<T>();
                if (_parent != null)
                {
                    o.transform.SetParent(_parent, false);
                }
                return c;
            };
        }

        public GameObjectPool(GameObject prefab, Transform parent, int maxSize) : this(prefab, maxSize)
        {
            _parent = parent;
        }

        public void Preload(int objectsCount)
        {
            if (objectsCount <= 0)
            {
                return;
            }
            var objects = new T[objectsCount];
            for (var i = 0; i < objectsCount; ++i)
            {
                objects[i] = Acquire();
            }
            
            for (int i = 0; i < objectsCount; ++i)
            {
                Release(objects[i]);
            }
        }

        public void SetParentForPoolObjects(Transform parent)
        {
            _parent = parent;
            for (var i = 0; i < Objects.Length; i++)
            {
                Objects[i].transform.SetParent(parent,false);
            }
        }

        void IDisposable.Dispose()
        {
            Reset();
        }
    }
}