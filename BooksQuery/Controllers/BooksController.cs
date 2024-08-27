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
        public async Task<IActionResult> GetAllBooks([FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "pageSize")] int pageSize = 10)
        {
            var books = await _mediator.Send(new GellAllBooksQuery.Query(page, pageSize));

            var booksResponse = books.ToList().ConvertAll(book => new BookResponse(book.BookId.ToString(), book.Title, book.IsReserved));

            return Ok(new BookResponseWithPaging(booksResponse, new Paging(page, pageSize)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById([FromRoute(Name = "id")] string id)
        {
            var query = new GetBookByIdQuery.Query(id);

            try
            {
                var book = await _mediator.Send(query);

                return Ok(book);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
