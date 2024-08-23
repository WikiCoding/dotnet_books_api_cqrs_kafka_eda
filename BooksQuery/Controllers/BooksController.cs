using BooksQuery.Contracts;
using BooksQuery.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BooksQuery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _mediator.Send(new GellAllBooksQuery.Query());

            return Ok(books.ToList().ConvertAll(book => new BookResponse(book.Id.ToString(), book.Title, book.IsReserved)));
        }
    }
}
