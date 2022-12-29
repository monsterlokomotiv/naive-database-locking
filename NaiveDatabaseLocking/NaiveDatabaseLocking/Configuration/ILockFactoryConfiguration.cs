namespace NaiveDatabaseLocking.Configuration;

public interface ILockFactoryConfiguration
{
    public Func<int, int> MillisecondsToWaitBetweenRetryCalculator { get; }
}
