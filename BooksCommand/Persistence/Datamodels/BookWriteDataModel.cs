﻿using System.ComponentModel.DataAnnotations;

namespace BooksCommand.Persistence.Datamodels
{
    public class BookWriteDataModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public bool IsReserved { get; set; } = false;
    }
}
