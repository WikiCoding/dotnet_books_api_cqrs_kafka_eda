namespace BooksCommand.Domain.ValueObjects
{
    public class BookId
    {
        public Guid Id { get; init; }

        public BookId(Guid id)
        {
            Id = id;
        }
    }
}
