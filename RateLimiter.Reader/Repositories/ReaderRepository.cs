using MongoDB.Driver;
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
    
    public async Task<List<RateLimit>> GetRateLimitsBatchAsync(int skip, int limit)
    {
        var rateLimitEntities = await _rateLimitsCollection
            .Find(_ => true)
            .Skip(skip)
            .Limit(limit)
            .ToListAsync();
        Console.WriteLine("Get batches");

        return rateLimitEntities.ConvertAll(entity => _rateLimitMapper.MapToRateLimit(entity));
    }
    
    public Task<IChangeStreamCursor<ChangeStreamDocument<RateLimitEntity>>> WatchRateLimitChanges()
    {
        var options = new ChangeStreamOptions
        {
            FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
        };

        return _rateLimitsCollection.WatchAsync(options);
    }
    
    public RateLimit MapChangeToRateLimit(ChangeStreamDocument<RateLimitEntity> change)
    {
        return _rateLimitMapper.MapToRateLimit(change.FullDocument);
    }
}