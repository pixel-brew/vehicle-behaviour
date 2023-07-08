namespace Core.Client
{
    public class ObjectPool<TObject>
    {
        public bool IsEnabled = true;

        private readonly int _capacity;
        public int Capacity => _capacity;
        public int ObjectsAmount { get; protected set; }
        protected readonly TObject[] Objects;
        private System.Func<TObject> _constructor;
        private System.Action<TObject> _disabler;
        private System.Action<TObject> _destroyer;
        private System.Action<TObject> _enabler;
        protected readonly object _locker = new object();

        public System.Func<TObject> Constructor
        {
            set => _constructor = value;
        }

        public System.Action<TObject> Disabler
        {
            set => _disabler = value;
        }

        public System.Action<TObject> Enabler
        {
            set => _enabler = value;
        }

        public System.Action<TObject> Destoyer
        {
            set => _destroyer = value;
        }

        public ObjectPool(int capacity)
        {
            _capacity = capacity;
            Objects = new TObject[_capacity];
        }

        public TObject Acquire(bool log = false)
        {
            lock (_locker)
            {
                TObject result;

                if (IsEnabled && ObjectsAmount > 0)
                {
                    --ObjectsAmount;
                    result = Objects[ObjectsAmount];
                    Objects[ObjectsAmount] = default(TObject);
                    if (_enabler != null)
                    {
                        _enabler.Invoke(result);
                    }
                }
                else
                {
                    result = _constructor.Invoke();
                }

                return result;
            }
        }

        public bool Release(TObject obj)
        {
            lock (_locker)
            {
                if (_disabler != null)
                {
                    _disabler.Invoke(obj);
                }

                if (!IsEnabled || ObjectsAmount >= _capacity)
                {
                    if (_destroyer != null)
                    {
                        _destroyer.Invoke(obj);
                    }

                    return false;
                }

                Objects[ObjectsAmount] = obj;
                ++ObjectsAmount;
                return true;
            }
        }

        public void Reset()
        {
            lock (_locker)
            {
                while (ObjectsAmount > 0)
                {
                    --ObjectsAmount;
                    var o = Objects[ObjectsAmount];
                    Objects[ObjectsAmount] = default(TObject);
                    if (_disabler != null)
                    {
                        _disabler.Invoke(o);
                    }

                    if (_destroyer != null)
                    {
                        _destroyer.Invoke(o);
                    }
                }
            }
        }
    }
}