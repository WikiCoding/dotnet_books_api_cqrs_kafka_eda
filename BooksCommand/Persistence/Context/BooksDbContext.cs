using BooksCommand.Persistence.Datamodels;
using Microsoft.EntityFrameworkCore;

namespace BooksCommand.Persistence.Context
{
    public class BooksDbContext(DbContextOptions<BooksDbContext> options) : DbContext(options)
    {
        public DbSet<BookWriteDataModel> Books { get; set; }
        public DbSet<BookOutBoxDataModel> OutBoxModels { get; set; }


    }
}
