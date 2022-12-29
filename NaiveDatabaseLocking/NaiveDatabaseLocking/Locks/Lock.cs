namespace NaiveDatabaseLocking.Locks;

public record Lock(Guid Id, string Key, DateTime ExpirationTime) : ILock
{
    //private 
}
