using BooksCommand.Domain.DDD;

namespace BooksCommand.Domain.ValueObjects
{
    public class BookIsReserved : IValueObject
    {
        public bool IsReserved { get; init; } = false;

    }
}
