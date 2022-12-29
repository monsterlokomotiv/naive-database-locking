using NaiveDatabaseLocking.Locks;

namespace NaiveDatabaseLocking.LockProviders;

public interface ILockProvider
{
    Task<ILockContainer> GetLock(string key);
    Task ReleaseLock(ILock toRelease);
}
