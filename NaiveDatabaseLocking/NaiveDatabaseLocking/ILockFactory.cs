using NaiveDatabaseLocking.Locks;

namespace NaiveDatabaseLocking;

public interface ILockFactory
{
    Task<ILock> CreateLock(string key);
    Task<ILock> CreateLock(string key, int millisecondsToWaitAtMax);
}
