using BooksCommand.Domain.DDD;

namespace BooksCommand.Domain.ValueObjects
{
    public class BookTitle : IValueObject
    {
        public string Title { get; init; } = string.Empty;

        public BookTitle(string title)
        {
            if (title.Trim().Length == 0) throw new ArgumentException("title is empty");
            Title = title;
        }
    }
}
