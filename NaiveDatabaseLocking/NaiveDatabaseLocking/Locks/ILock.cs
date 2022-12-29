namespace NaiveDatabaseLocking.Locks;

public interface ILock
{
    Guid Id { get; }
    string Key { get; }
    DateTime ExpirationTime { get; }
}
