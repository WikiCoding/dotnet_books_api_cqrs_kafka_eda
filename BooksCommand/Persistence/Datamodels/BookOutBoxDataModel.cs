using System.ComponentModel.DataAnnotations;

namespace BooksCommand.Persistence.Datamodels
{
    public class BookOutBoxDataModel
    {
        [Key]
        public int StreamId { get; set; }
        [Required]
        public Guid BookId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public bool IsReserved { get; set; } = true;
        public EventType EventType { get; set; } // make enum or string
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedDate { get; set; }
    }
}
