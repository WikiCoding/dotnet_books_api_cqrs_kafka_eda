namespace BooksCommand.Events
{
    public class CreatedBookEvent : IDomainEvent
    {
        //public int StreamId { get; set; }
        public Guid BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsReserved { get; set; } = false;
        public bool IsCreationEvent { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}
