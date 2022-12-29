using NaiveDatabaseLocking.LockProviders;

namespace NaiveDatabaseLocking.Locks;

public interface ILockContainer
{
    ILock? Lock { get; }
    LockCreationStatus Status { get; }
}
