using AutoMapper;
using BrainBridgePrototype.Controllers;
using BrainBridgePrototype.DTOs;
using BrainBridgePrototype.Models;
using BrainBridgePrototype.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace BrainBridgePrototype.Tests
{
    public class CommentControllerTests
    {
        private readonly Mock<ICommentService> _mockCommentService;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CommentController _controller;

        public CommentControllerTests()
        {
            _mockCommentService = new Mock<ICommentService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new CommentController(_mockCommentService.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task UpdateComment_ValidId_ReturnsOk()
        {
            // Arrange
            var userId = "user1"; // Example user ID
            var commentDto = new CommentDto { Content = "Updated content" };
            var commentId = 1; // Example comment ID

            var comment = new Comment { Id = commentId, Content = "Original content", UserId = userId };
            _mockCommentService.Setup(x => x.GetCommentById(commentId)).ReturnsAsync(comment);

            var updatedComment = new Comment { Id = commentId, Content = commentDto.Content, UserId = userId };
            _mockCommentService.Setup(x => x.UpdateComment(commentId, commentDto, userId)).ReturnsAsync(updatedComment);

            // Mocking User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                // Add roles if necessary
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = await _controller.UpdateComment(commentId, commentDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultComment = Assert.IsAssignableFrom<Comment>(okResult.Value);
            Assert.Equal(updatedComment.Id, resultComment.Id);
            Assert.Equal(updatedComment.Content, resultComment.Content);
            Assert.Equal(updatedComment.UserId, resultComment.UserId);
        }

        [Fact]
        public async Task GetComment_ExistingId_ReturnsOk()
        {
            // Arrange
            var commentId = 1;
            var comment = new Comment { Id = commentId, Content = "Test Comment" };
            _mockCommentService.Setup(s => s.GetCommentById(commentId)).ReturnsAsync(comment);

            // Act
            var result = await _controller.GetComment(commentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Comment>(okResult.Value);
            Assert.Equal(commentId, model.Id);
        }

        [Fact]
        public async Task GetComment_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var commentId = 999;
            _mockCommentService.Setup(s => s.GetCommentById(commentId)).ReturnsAsync((Comment)null);

            // Act
            var result = await _controller.GetComment(commentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateComment_ValidDto_ReturnsOk()
        {
            // Arrange
            var postId = 1;
            var commentDto = new CommentDto { Content = "New Comment" };
            var createdComment = new Comment { Id = 1, Content = commentDto.Content };
            _mockCommentService.Setup(s => s.CreateComment(commentDto, It.IsAny<string>(), postId)).ReturnsAsync(createdComment);

            // Mocking User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "testUserId"),
                // Add roles if necessary
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = await _controller.CreateComment(postId, commentDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Comment>(okResult.Value);
            Assert.Equal(createdComment.Id, model.Id);
        }

        [Fact]
        public async Task UpdateComment_ValidIdAndDto_ReturnsOk()
        {
            // Arrange
            var commentId = 1;
            var commentDto = new CommentDto { Content = "Updated Comment" };
            var userId = "testUserId";
            var existingComment = new Comment { Id = commentId, Content = "Original Comment", UserId = userId };
            _mockCommentService.Setup(s => s.GetCommentById(commentId)).ReturnsAsync(existingComment);
            _mockCommentService.Setup(s => s.UpdateComment(commentId, commentDto, userId)).ReturnsAsync(existingComment);

            // Mocking User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                // Add roles if necessary
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = await _controller.UpdateComment(commentId, commentDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Comment>(okResult.Value);
            Assert.Equal(existingComment.Id, model.Id);
        }

        [Fact]
        public async Task UpdateComment_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var commentId = 999;
            var commentDto = new CommentDto { Content = "Updated Comment" };

            // Mocking User
            var userId = "testUserId";
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId),
                // Add roles if necessary
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = await _controller.UpdateComment(commentId, commentDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task UpdateComment_UnauthorizedUser_ReturnsForbid()
        {
            // Arrange
            var commentId = 1;
            var commentDto = new CommentDto { Content = "Updated Comment" };
            var userId = "unauthorizedUserId";
            var existingComment = new Comment { Id = commentId, Content = "Original Comment", UserId = "authorizedUserId" };
            _mockCommentService.Setup(s => s.GetCommentById(commentId)).ReturnsAsync(existingComment);

            // Mocking User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                // Add roles if necessary
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = await _controller.UpdateComment(commentId, commentDto);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task DeleteComment_ValidId_ReturnsOk()
        {
            // Arrange
            var commentId = 1;

            // Act
            var result = await _controller.DeleteComment(commentId);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
