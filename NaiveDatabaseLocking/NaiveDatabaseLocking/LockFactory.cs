using Microsoft.Extensions.Logging;
using NaiveDatabaseLocking.Configuration;
using NaiveDatabaseLocking.LockProviders;
using NaiveDatabaseLocking.Locks;

namespace NaiveDatabaseLocking;

public class LockFactory : ILockFactory
{
    private readonly ILockFactoryConfiguration _configuration;
    private readonly ILockProvider _lockProvider;
    private readonly ILogger _logger;

    public LockFactory(ILockFactoryConfiguration configuration,
        ILockProvider lockProvider,
        ILogger<LockFactory> logger)
    {
        _configuration = configuration;
        _lockProvider = lockProvider;
        _logger = logger;
    }

    public Task<ILock> CreateLock(string key)
    {
        return CreateLock(key, 500);
    }

    public async Task<ILock> CreateLock(string key, int millisecondsToWaitAtMax)
    {
        var cancTokenSource = new CancellationTokenSource();
        cancTokenSource.CancelAfter(millisecondsToWaitAtMax);

        var createdLock = await AttemptToGetLock(key, cancTokenSource.Token);
        return createdLock;
    }

    private async Task<ILock> AttemptToGetLock(string key, CancellationToken token)
    {
        var numberOfAttempts = 0;
        while (!token.IsCancellationRequested)
        {
            var lockFetchAttempt = await _lockProvider.GetLock(key);
            if (lockFetchAttempt.Status == LockCreationStatus.Created && lockFetchAttempt.Lock != null)
                return lockFetchAttempt.Lock;
            else if (lockFetchAttempt.Status == LockCreationStatus.Created)
                throw new LockCreationFailureException(key, "The lock provider gave status Created but did not give back a lock");

            numberOfAttempts++;
            await Task.Delay(_configuration.MillisecondsToWaitBetweenRetryCalculator(numberOfAttempts), token);
        }

        _logger.LogDebug("Attempted {numberOfAttempts} times to fetch lock for key {key} before running out of time.", numberOfAttempts, key);
        throw new LockCreationFailureException(key, "Failed to get a lock within the alloted time");
    }

    public async Task ReleaseLock(ILock toRelease)
    {
        await _lockProvider.ReleaseLock(toRelease);
    }
}
