using System.ComponentModel.DataAnnotations;

namespace BooksCommand.Persistence
{
    public class BookOutBoxDataModel
    {
        [Key]
        public int StreamId { get; set; }
        [Required]
        public Guid BookId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public bool IsReserved { get; set; } = false;
        public bool IsCreationEvent { get; set; } = true;
        //public bool IsUpdateEvent { get; set; } = false;
        //public bool IsDeleteEvent { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedDate { get; set; }
    }
}
