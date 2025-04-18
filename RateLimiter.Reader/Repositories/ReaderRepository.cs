using System.Runtime.CompilerServices;
using MongoDB.Driver;
using RateLimiter.Reader.CustomExceptions;
using RateLimiter.Reader.Models;
using RateLimiter.Reader.Models.Entities;
using RateLimiter.Reader.Services;

namespace RateLimiter.Reader.Repositories;

public class ReaderRepository : IReaderRepository
{
    private readonly IMongoCollection<RateLimitEntity> _rateLimitsCollection;
    private readonly IRateLimitEntityToRateLimitMapper _rateLimitMapper;

    public ReaderRepository(DbService mongoDbService, IRateLimitEntityToRateLimitMapper rateLimitMapper)
    {
        _rateLimitsCollection = mongoDbService.GetCollection<RateLimitEntity>("rate_limits");
        _rateLimitMapper = rateLimitMapper;
    }

    public async IAsyncEnumerable<(string id, RateLimit rateLimit)> GetRateLimitsBatchAsync(int batchSize)
    {
        var filter = Builders<RateLimitEntity>.Filter.Empty;
        var options = new FindOptions<RateLimitEntity, RateLimitEntity>
        {
            BatchSize = batchSize
        };

        using var cursor = await _rateLimitsCollection.FindAsync(filter: filter, options: options);

        while (await cursor.MoveNextAsync())
        {
            foreach (var rateLimitEntity in cursor.Current)
            {
                var rateLimit = _rateLimitMapper.MapToRateLimit(rateLimitEntity);
                yield return (rateLimitEntity.Id, rateLimit);
            }
        }
    }

    public async IAsyncEnumerable<(ChangeStreamOperationType OperationType, string? id, RateLimit? RateLimit)>
        WatchRateLimitChangesAsync(
            [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var options = new ChangeStreamOptions
        {
            FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
        };

        using var changeStream = await _rateLimitsCollection.WatchAsync(options, cancellationToken);

        while (await changeStream.MoveNextAsync(cancellationToken))
        {
            foreach (var change in changeStream.Current)
            {
                if (change.OperationType is not 
                    (ChangeStreamOperationType.Update
                    or ChangeStreamOperationType.Delete
                    or ChangeStreamOperationType.Insert))
                    throw new UnsupportedOperationTypeException($"Unsupported operation type: {change.OperationType}");
                
                var rateLimit = change.OperationType is ChangeStreamOperationType.Delete ? null : _rateLimitMapper.MapToRateLimit(change.FullDocument);
                yield return (change.OperationType, change.DocumentKey["_id"].ToString(), rateLimit);
            }
        }
    }
}