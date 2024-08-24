using BooksCommand.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static BooksCommand.Commands.CreateBook;

namespace BooksCommand.Controllers
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

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookCommand command)
        {
            try
            {
                var bookDm = await _mediator.Send(command);

                return CreatedAtAction(nameof(CreateBook), bookDm);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return Problem(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ReserveBook([FromRoute(Name = "id")] int id)
        {
            var reserveBookCommand = new ReserveBook.ReserveBookCommand() { BookId = id };
            var bookDm = await _mediator.Send(reserveBookCommand);

            return Ok(bookDm);
        }

    }
}
