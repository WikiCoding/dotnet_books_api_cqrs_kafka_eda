namespace BooksCommand.Domain.ValueObjects
{
    public class BookId
    {
        public int Id { get; init; }

        public BookId(int id)
        {
            if (id < 0) throw new ArgumentException("invalid id value");
            Id = id;
        }
    }
}
