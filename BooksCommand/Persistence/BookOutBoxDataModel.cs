using System.ComponentModel.DataAnnotations;

namespace BooksCommand.Persistence
{
    public class BookOutBoxDataModel
    {
        [Key]
        public int StreamId { get; set; }
        //[Required]
        //public int BookId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public bool IsReserved { get; set; } = false;
        public bool IsCreationEvent { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}
