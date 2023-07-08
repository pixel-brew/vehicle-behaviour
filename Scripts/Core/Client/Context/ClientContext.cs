using System;
using System.Collections.Generic;
using System.Reflection;

using Cysharp.Threading.Tasks;

namespace Core.Client.Context
{
    public class ClientContext : IClientContext
    {
        private event Action OnStartLoadContext;
        private event Action OnFinishLoadContext;
        private event Action OnStartUnloadContext;
        private event Action OnFinishUnloadContext;

        // private readonly UpdateFrameHandlerDispatcher _updateHandlerDispatcher = new UpdateFrameHandlerDispatcher();
        private readonly CustomHandlerDispatcher _customHandlerDispatcher = new CustomHandlerDispatcher();
        private IList<IClientContextModule> _modules;
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        
        IList<CustomHandlerDispatcher.HandlerInfo> GetBaseHandlers()
        {
            return new List<CustomHandlerDispatcher.HandlerInfo>()
            {
                new CustomHandlerDispatcher.HandlerInfo()
                {
                    AttributeType = typeof(StartLoadContextHandlerAttribute),
                    AttachHandler = d => OnStartLoadContext += (Action) d,
                    HandlerType = typeof(Action)
                },
                new CustomHandlerDispatcher.HandlerInfo()
                {
                    AttributeType = typeof(FinishLoadContextHandlerAttribute),
                    AttachHandler = d => OnFinishLoadContext += (Action) d,
                    HandlerType = typeof(Action)
                },
                new CustomHandlerDispatcher.HandlerInfo()
                {
                    AttributeType = typeof(StartUnloadContextHandlerAttribute),
                    AttachHandler = d => OnStartUnloadContext += (Action) d,
                    HandlerType = typeof(Action)
                },
                new CustomHandlerDispatcher.HandlerInfo()
                {
                    AttributeType = typeof(FinishUnloadContextHandlerAttribute),
                    AttachHandler = d => OnFinishUnloadContext += (Action) d,
                    HandlerType = typeof(Action)
                }
            };
        }
        private IList<IClientContextModule> FindModules()
        {
            var result = new List<IClientContextModule>();
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var hierarchy = GetType().GetHierarchy();
            var iContextModuleName = typeof(IClientContextModule).Name;
            foreach (var type in hierarchy)
            {
                var fields = type.GetFields(bindingFlags);
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(IClientContextModule) || field.FieldType.GetInterface(iContextModuleName) != null)
                    {
                        var module = (IClientContextModule) field.GetValue(this);
                        result.Add(module);
                    }
                }
            }
            return result;
        }

        private void AttachModule(IClientContextModule module)
        {
            module.Initialize(this);
            // _customHandlerDispatcher.ProcessCustomHandlers(module, module.Handlers);
            // _updateHandlerDispatcher.AttachHandlersToObject(module, _scheduler);
        }

        async UniTask<bool> IClientContext.Load()
        {
            OnStartLoadContext?.Invoke();

            _customHandlerDispatcher.AddHandlers(GetBaseHandlers());
            _customHandlerDispatcher.AttachCurrentObjectToAllHandlers(this);

            _modules = FindModules();
            foreach (var module in _modules)
            {
                AttachModule(module);
            }
            
            foreach (var module in _modules)
            {
                await module.Load();
            }
            
            OnFinishLoadContext?.Invoke();
            
            new ContextLoadedEvent(this).Launch();
            
            return true;
        }

        async UniTask<bool> IClientContext.Unload()
        {
            
            OnStartUnloadContext?.Invoke();
            foreach (var module in _modules)
            {
                await module.Unload();
            }
            _modules = null;
            foreach (var disposable in _disposables)
            {
                disposable?.Dispose();
            }
            _disposables.Clear();
            OnFinishUnloadContext?.Invoke();
            
            new ContextUnloadedEvent(this).Launch();
            
            _customHandlerDispatcher.Clear();
            
            return true;
        }

        public T GetModule<T>() where T : class, IClientContextModule
        {
            T result = null;
            if (_modules != null)
            {
                for (int i = 0, max = _modules.Count; i < max; ++i)
                {
                    result = _modules[i] as T;
                    if (result != null)
                    {
                        break;
                    }
                }
            }
            
            return result;
        }

        public void AddDisposable(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }
    }
}