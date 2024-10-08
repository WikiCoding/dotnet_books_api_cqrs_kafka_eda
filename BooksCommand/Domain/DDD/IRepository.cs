﻿namespace BooksCommand.Domain.DDD
{
    public interface IRepository<T, K>
    {
        Task<K> SaveBook(T aggregateRoot, CancellationToken cancellationToken);
    }
}
