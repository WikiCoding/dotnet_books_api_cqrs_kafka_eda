using BooksQuery.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace BooksQuery.Database
{
    public class BookReadDbContext(DbContextOptions<BookReadDbContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().ToCollection("books_query_db");
        }
    }
}
