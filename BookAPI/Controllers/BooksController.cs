using BookAPI.DTO;
using BookAPI.Models;
using BookAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet("books")]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _bookRepository.GetAllBooksAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("books/{isbn}")]
        public async Task<IActionResult> GetBooksByIsbn(string isbn)
        {
            try
            {
                var book = await _bookRepository.GetBookByIsbnAsync(isbn);
                if (book == null)
                {
                    return NotFound("Book not found");
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("add-book")]
        public async Task<IActionResult> AddBook([FromBody] BookDTO bookDto)
        {
            try
            {
                var Book = new Book
                {
                    ISBN = bookDto.ISBN,
                    Title = bookDto.Title,
                    Author = bookDto.Author,
                    PublicationYear = bookDto.PublicationYear
                };

                await _bookRepository.AddBookAsync(Book);
                return Ok("Book added successfully !");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update-book/{isbn}")]
        public async Task<IActionResult> UpdateBook(string isbn, [FromBody] BookDTO bookDto)
        {
            try
            {
                var book = await _bookRepository.GetBookByIsbnAsync(isbn);
                if (book == null)
                {
                    return NotFound("Book not found");
                }

                //book.ISBN = bookDto.ISBN;
                book.Title = bookDto.Title;
                book.Author = bookDto.Author;
                book.PublicationYear = bookDto.PublicationYear;

                await _bookRepository.UpdateBookAsync(book);
                return Ok("Book updated successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("delete-book/{isbn}")]
        public async Task<IActionResult> DeleteBook(string isbn)
        {
            try
            {
                var book = await _bookRepository.GetBookByIsbnAsync(isbn);
                if (book == null)
                {
                    return NotFound("Book not found");
                }
                await _bookRepository.DeleteBookByIsbnAsync(isbn);
                return Ok("Book deleted successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
