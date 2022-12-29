namespace NaiveDatabaseLocking;

public class LockCreationFailureException : Exception
{
    public LockCreationFailureException(string key, string reason) : base($"Failed to create a lock instance for key \"{key}\" due to reason: {reason}.") { }
    public LockCreationFailureException(string key, Exception ex) : base($"Failed to create a lock instance for key \"{key}\" due to exception: {ex.Message}.", ex) { }
}
