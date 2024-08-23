using BooksCommand.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BooksCommand.Database
{
    public class BooksDbContext(DbContextOptions<BooksDbContext> options) : DbContext(options)
    {
        public DbSet<BookWriteDataModel> Books { get; set; }
        public DbSet<BookOutBoxDataModel> OutBoxModels { get; set; }
    }
}
