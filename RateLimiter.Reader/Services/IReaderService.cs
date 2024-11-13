﻿using RateLimiter.Reader.Models;

namespace RateLimiter.Reader.Services;

public interface IReaderService
{
    IAsyncEnumerable<RateLimit> GetAllRateLimitsAsync();
    Task AddRateLimitAsync(RateLimit rateLimit);
    Task UpdateRateLimitAsync(RateLimit rateLimit);
    Task RemoveRateLimitAsync(string route);
}