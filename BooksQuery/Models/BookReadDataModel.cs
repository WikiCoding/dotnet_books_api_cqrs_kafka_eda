namespace BooksQuery.Models
{
    public class BookReadDataModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsReserved { get; set; } = false;
    }
}
