namespace Core.Client
{
    public interface IPoolable
    {
        void Enable();
        void Disable();
        void Destroy();
    }
}