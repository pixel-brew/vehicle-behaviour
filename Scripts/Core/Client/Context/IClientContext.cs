using System;
using Cysharp.Threading.Tasks;

namespace Core.Client.Context
{
    public interface IClientContext 
    {
        UniTask<bool> Load();
        UniTask<bool> Unload();
        
        T GetModule<T>() where T : class, IClientContextModule;
        
        void AddDisposable(IDisposable disposable);
    }
}