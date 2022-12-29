namespace NaiveDatabaseLocking.Configuration;

internal class DefaultLockFactoryConfiguration : ILockFactoryConfiguration
{
    private static Func<int, int> DefaultMillisecondsToWaitCalculator = retryCounter => 1 + ((retryCounter / 10) * 1);
    public Func<int, int> MillisecondsToWaitBetweenRetryCalculator { get; set; } = DefaultMillisecondsToWaitCalculator;
}
