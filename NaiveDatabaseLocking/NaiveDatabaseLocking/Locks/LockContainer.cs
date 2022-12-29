using NaiveDatabaseLocking.LockProviders;

namespace NaiveDatabaseLocking.Locks;

public record LockContainer(ILock? Lock, LockCreationStatus Status) : ILockContainer;
