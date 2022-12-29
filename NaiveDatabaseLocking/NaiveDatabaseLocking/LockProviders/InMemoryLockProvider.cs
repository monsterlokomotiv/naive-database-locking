using Microsoft.Extensions.Logging;
using NaiveDatabaseLocking.Locks;
using System.Collections.Concurrent;

namespace NaiveDatabaseLocking.LockProviders;

public class InMemoryLockProvider : ILockProvider
{
    private const int LockDurationInSeconds = 5;
    private readonly ConcurrentDictionary<string, Guid> _keyContainer = new();
    private readonly ILogger _logger;

    public InMemoryLockProvider(ILogger<InMemoryLockProvider> logger)
    {
        _logger = logger;
    }

    public Task<ILockContainer> GetLock(string key)
    {
        _logger.LogDebug("Attempting to create lock for key {key}", key);
        if (_keyContainer.ContainsKey(key))
        {

            return Task.FromResult<ILockContainer>(new LockContainer(null, LockCreationStatus.AlreadyExists));
        }

        var lockId = Guid.NewGuid();
        if(_keyContainer.TryAdd(key, lockId))
        {
            var createdLock = new Lock(lockId, key, DateTime.UtcNow.AddSeconds(LockDurationInSeconds));
            return Task.FromResult<ILockContainer>(new LockContainer(createdLock, LockCreationStatus.Created));
        }

        return Task.FromResult<ILockContainer>(new LockContainer(null, LockCreationStatus.AlreadyExists));
    }

    public Task ReleaseLock(ILock toRelease)
    {
        _logger.LogDebug("Starting release process of lock with id {Id} - created for key {Key}", toRelease.Id, toRelease.Key);
        if (!_keyContainer.ContainsKey(toRelease.Key))
        {
            _logger.LogWarning("Attempted to release lock with id {Id} but it was already removed!", toRelease.Id);
            return Task.CompletedTask;
        }

        _keyContainer.Remove(toRelease.Key, out _);

        _logger.LogDebug("Successfully released lock with id {Id} for key {Key}", toRelease.Id, toRelease.Key);
        return Task.CompletedTask;
    }
}
