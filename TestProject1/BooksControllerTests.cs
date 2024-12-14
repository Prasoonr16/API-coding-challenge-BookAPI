using Moq;
using NUnit.Framework;
using BookAPI.Controllers;
using BookAPI.Models;
using BookAPI.Repositories;
using BookAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentAssertions;

namespace TestProject1
{
    [TestFixture]
    public class BooksControllerTests
    {
        private Mock<IBookRepository> _bookRepositoryMock;
        private BooksController _booksController;

        [SetUp]
        public void SetUp()
        {
            // Mock the IBookRepository
            _bookRepositoryMock = new Mock<IBookRepository>();
            _booksController = new BooksController(_bookRepositoryMock.Object);
        }

        // Test: Get All Books
        [Test]
        public async Task GetAllBooks_ReturnsOkResult_WhenBooksExist()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { ISBN = 1, Title = "Book 1", Author = "Author 1", PublicationYear = 2021 },
                new Book { ISBN = 2, Title = "Book 2", Author = "Author 2", PublicationYear = 2022 }
            };

            _bookRepositoryMock.Setup(repo => repo.GetAllBooksAsync()).ReturnsAsync(books);

            // Act
            var result = await _booksController.GetAllBooks();

            // Assert
            result.Should().BeOfType<OkObjectResult>();  // Check if result is Ok
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(books);  // Verify the returned books list
        }

        // Test: Get Book by ISBN
        [Test]
        public async Task GetBooksByIsbn_ReturnsOkResult_WhenBookExists()
        {
            // Arrange
            var book = new Book { ISBN = 1, Title = "Book 1", Author = "Author 1", PublicationYear = 2021 };

            _bookRepositoryMock.Setup(repo => repo.GetBookByIsbnAsync(1)).ReturnsAsync(book);

            // Act
            var result = await _booksController.GetBooksByIsbn(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();  // Check if result is Ok
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(book);  // Verify the returned book
        }

        [Test]
        public async Task GetBooksByIsbn_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            _bookRepositoryMock.Setup(repo => repo.GetBookByIsbnAsync(1)).ReturnsAsync((Book)null);

            // Act
            var result = await _booksController.GetBooksByIsbn(1);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();  // Check if result is NotFound
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Value.Should().Be("Book not found");  // Verify the error message
        }

        // Test: Add Book
        [Test]
        public async Task AddBook_ReturnsOkResult_WhenBookIsAdded()
        {
            // Arrange
            var bookDto = new BookDTO
            {
                ISBN = 3,
                Title = "Book 3",
                Author = "Author 3",
                PublicationYear = 2023
            };

            // Act
            var result = await _booksController.AddBook(bookDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();  // Check if result is Ok
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be("Book added successfully !");  // Verify the success message
            _bookRepositoryMock.Verify(repo => repo.AddBookAsync(It.IsAny<Book>()), Times.Once);  // Ensure AddBookAsync was called
        }

        // Test: Update Book
        [Test]
        public async Task UpdateBook_ReturnsOkResult_WhenBookIsUpdated()
        {
            // Arrange
            var bookDto = new BookDTO
            {
                ISBN = 1,
                Title = "Updated Book 1",
                Author = "Updated Author 1",
                PublicationYear = 2024
            };
            var book = new Book { ISBN = 1, Title = "Book 1", Author = "Author 1", PublicationYear = 2021 };
            _bookRepositoryMock.Setup(repo => repo.GetBookByIsbnAsync(1)).ReturnsAsync(book);

            // Act
            var result = await _booksController.UpdateBook(1, bookDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();  // Check if result is Ok
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be("Book updated successfully!");  // Verify the success message
            _bookRepositoryMock.Verify(repo => repo.UpdateBookAsync(It.IsAny<Book>()), Times.Once);  // Ensure UpdateBookAsync was called
        }

        [Test]
        public async Task UpdateBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var bookDto = new BookDTO { ISBN = 1, Title = "Updated Book", Author = "Updated Author", PublicationYear = 2023 };
            _bookRepositoryMock.Setup(repo => repo.GetBookByIsbnAsync(1)).ReturnsAsync((Book)null);

            // Act
            var result = await _booksController.UpdateBook(1, bookDto);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();  // Check if result is NotFound
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Value.Should().Be("Book not found");  // Verify the error message
        }

        // Test: Delete Book
        [Test]
        public async Task DeleteBook_ReturnsOkResult_WhenBookIsDeleted()
        {
            // Arrange
            var book = new Book { ISBN = 1, Title = "Book 1", Author = "Author 1", PublicationYear = 2021 };
            _bookRepositoryMock.Setup(repo => repo.GetBookByIsbnAsync(1)).ReturnsAsync(book);

            // Act
            var result = await _booksController.DeleteBook(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();  // Check if result is Ok
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be("Book deleted successfully!");  // Verify the success message
            _bookRepositoryMock.Verify(repo => repo.DeleteBookByIsbnAsync(1), Times.Once);  // Ensure DeleteBookByIsbnAsync was called
        }

        [Test]
        public async Task DeleteBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            _bookRepositoryMock.Setup(repo => repo.GetBookByIsbnAsync(1)).ReturnsAsync((Book)null);

            // Act
            var result = await _booksController.DeleteBook(1);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();  // Check if result is NotFound
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Value.Should().Be("Book not found");  // Verify the error message
        }
    }
}
