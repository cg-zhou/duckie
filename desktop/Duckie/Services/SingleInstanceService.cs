using System.Security.Principal;
using System.Threading;

namespace Duckie.Services;

/// <summary>One instance per Windows user and session, with activation via a named event.</summary>
internal sealed class SingleInstanceService : IDisposable
{
    private readonly Mutex mutex;
    private readonly EventWaitHandle activationEvent;
    private readonly RegisteredWaitHandle activationWait;
    private bool disposed;

    public bool IsPrimary { get; }

    public SingleInstanceService(Action activate, string applicationId = "Duckie")
    {
        var name = @"Local\" + applicationId + "." + WindowsIdentity.GetCurrent().User.Value;
        // Retain activation requests even while the primary instance is starting.
        activationEvent = new EventWaitHandle(false, EventResetMode.AutoReset, name + ".Activate");
        mutex = new Mutex(false, name + ".Instance");
        try
        {
            IsPrimary = mutex.WaitOne(0);
        }
        catch (AbandonedMutexException)
        {
            IsPrimary = true;
        }

        if (IsPrimary)
            activationWait = ThreadPool.RegisterWaitForSingleObject(
                activationEvent, (_, _) => activate(), null, Timeout.Infinite, false);
        else
            activationEvent.Set();
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        activationWait?.Unregister(null);
        activationEvent.Dispose();
        if (IsPrimary) mutex.ReleaseMutex();
        mutex.Dispose();
    }
}
