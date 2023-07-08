using System;

namespace Core
{
    public interface IEventObserver : IDisposable
    {
        int EventId { get; }
        bool OnEvent(IEvent e);
    }
}