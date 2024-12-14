using BookAPI.Controllers;
using BookAPI.DTO;
using BookAPI.Models;
using BookAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookAPITest
{
    [TestFixture]
    public class BooksControllerTests
    {
        private Mock<IBookRepository> _mockBookRepository;
        private BooksController _controller;

        [SetUp]
        public void Setup()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _controller = new BooksController(_mockBookRepository.Object);
        }

        [Test]
        public async Task GetAllBooks_ShouldReturnOkResult_WithListOfBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { ISBN = "123", Title = "Test Book 1", Author = "Author 1", PublicationYear = 2021 },
                new Book { ISBN = "456", Title = "Test Book 2", Author = "Author 2", PublicationYear = 2020 }
            };
            _mockBookRepository.Setup(repo => repo.GetAllBooksAsync()).ReturnsAsync(books);

            // Act
            var result = await _controller.GetAllBooks();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(books, okResult.Value);
        }

        [Test]
        public async Task GetBooksByIsbn_ValidIsbn_ShouldReturnOkResult_WithBook()
        {
            // Arrange
            var book = new Book { ISBN = "123", Title = "Test Book", Author = "Author", PublicationYear = 2021 };
            _mockBookRepository.Setup(repo => repo.GetBookByIsbnAsync("123")).ReturnsAsync(book);

            // Act
            var result = await _controller.GetBooksByIsbn("123");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(book, okResult.Value);
        }

        [Test]
        public async Task GetBooksByIsbn_InvalidIsbn_ShouldReturnNotFoundResult()
        {
            // Arrange
            _mockBookRepository.Setup(repo => repo.GetBookByIsbnAsync("123")).ReturnsAsync((Book)null);

            // Act
            var result = await _controller.GetBooksByIsbn("123");

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task AddBook_ValidBook_ShouldReturnOkResult()
        {
            // Arrange
            var bookDto = new BookDTO
            {
                ISBN = "123",
                Title = "New Book",
                Author = "Author",
                PublicationYear = 2023
            };

            _mockBookRepository.Setup(repo => repo.AddBookAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddBook(bookDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual("Book added successfully !", okResult.Value);
        }

        [Test]
        public async Task UpdateBook_ValidIsbn_ShouldReturnOkResult()
        {
            // Arrange
            var bookDto = new BookDTO
            {
                Title = "Updated Book",
                Author = "Updated Author",
                PublicationYear = 2022
            };

            var book = new Book
            {
                ISBN = "123",
                Title = "Old Book",
                Author = "Old Author",
                PublicationYear = 2021
            };

            _mockBookRepository.Setup(repo => repo.GetBookByIsbnAsync("123")).ReturnsAsync(book);
            _mockBookRepository.Setup(repo => repo.UpdateBookAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateBook("123", bookDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual("Book updated successfully!", okResult.Value);
        }

        [Test]
        public async Task DeleteBook_ValidIsbn_ShouldReturnOkResult()
        {
            // Arrange
            var book = new Book { ISBN = "123", Title = "Test Book", Author = "Author", PublicationYear = 2021 };
            _mockBookRepository.Setup(repo => repo.GetBookByIsbnAsync("123")).ReturnsAsync(book);
            _mockBookRepository.Setup(repo => repo.DeleteBookByIsbnAsync("123")).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteBook("123");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual("Book deleted successfully!", okResult.Value);
        }

        [Test]
        public async Task DeleteBook_InvalidIsbn_ShouldReturnNotFoundResult()
        {
            // Arrange
            _mockBookRepository.Setup(repo => repo.GetBookByIsbnAsync("123")).ReturnsAsync((Book)null);

            // Act
            var result = await _controller.DeleteBook("123");

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
    }
}
