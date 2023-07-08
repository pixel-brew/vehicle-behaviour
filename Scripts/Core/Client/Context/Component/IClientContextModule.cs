
using Cysharp.Threading.Tasks;

namespace Core.Client.Context
{
    public interface IClientContextModule
    {
        void Initialize(IClientContext context);
        UniTask Load();
        UniTask Unload();
    }
}