using System.ComponentModel.DataAnnotations;

namespace BooksCommand.Persistence
{
    public class BookWriteDataModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public bool IsReserved { get; set; } = false;
    }
}
