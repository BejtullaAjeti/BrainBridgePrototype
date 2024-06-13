using AutoMapper;
using BrainBridgePrototype.Controllers;
using BrainBridgePrototype.DTOs;
using BrainBridgePrototype.Models;
using BrainBridgePrototype.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace BrainBridgePrototype.Tests
{
    public class PostControllerTests
    {
        private readonly Mock<IPostService> _mockPostService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PostController _controller;

        public PostControllerTests()
        {
            _mockPostService = new Mock<IPostService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new PostController(_mockPostService.Object, _mockMapper.Object);

            // Mock User identity
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "testUserId"),
                new Claim(ClaimTypes.Role, "Admin")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task GetAllPosts_ReturnsOk()
        {
            // Arrange
            var posts = new List<Post> { new Post { Id = 1, Title = "Test Post", Content = "Test Content" } };
            _mockPostService.Setup(s => s.GetAllPosts()).ReturnsAsync(posts);

            // Act
            var result = await _controller.GetAllPosts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Post>>(okResult.Value);
            Assert.Single(model);
        }

        [Fact]
        public async Task GetPost_ExistingId_ReturnsOk()
        {
            // Arrange
            var postId = 1;
            var post = new Post { Id = postId, Title = "Test Post", Content = "Test Content" };
            _mockPostService.Setup(s => s.GetPostById(postId)).ReturnsAsync(post);

            // Act
            var result = await _controller.GetPost(postId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Post>(okResult.Value);
            Assert.Equal(postId, model.Id);
        }

        [Fact]
        public async Task GetPost_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var postId = 999;
            _mockPostService.Setup(s => s.GetPostById(postId)).ReturnsAsync((Post)null);

            // Act
            var result = await _controller.GetPost(postId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreatePost_ValidDto_ReturnsOk()
        {
            // Arrange
            var postDto = new PostDto { Title = "New Post", Content = "New Content" };
            var createdPost = new Post { Id = 1, Title = postDto.Title, Content = postDto.Content };
            _mockPostService.Setup(s => s.CreatePost(It.IsAny<PostDto>(), It.IsAny<string>())).ReturnsAsync(createdPost);

            // Act
            var result = await _controller.CreatePost(postDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Post>(okResult.Value);
            Assert.Equal(createdPost.Id, model.Id);
        }

        [Fact]
        public async Task UpdatePost_ValidIdAndDto_ReturnsOk()
        {
            // Arrange
            var postId = 1;
            var postDto = new PostDto { Title = "Updated Post", Content = "Updated Content" };
            var userId = "testUserId";
            var existingPost = new Post { Id = postId, Title = "Original Post", Content = "Original Content", UserId = userId };
            _mockPostService.Setup(s => s.GetPostById(postId)).ReturnsAsync(existingPost);
            _mockPostService.Setup(s => s.UpdatePost(postId, postDto, userId)).ReturnsAsync(existingPost);

            // Act
            var result = await _controller.UpdatePost(postId, postDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Post>(okResult.Value);
            Assert.Equal(existingPost.Id, model.Id);
        }

        [Fact]
        public async Task UpdatePost_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var postId = 999;
            var postDto = new PostDto { Title = "Updated Post", Content = "Updated Content" };

            // Act
            var result = await _controller.UpdatePost(postId, postDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdatePost_UnauthorizedUser_ReturnsForbid()
        {
            // Arrange
            var postId = 1;
            var postDto = new PostDto { Title = "Updated Post", Content = "Updated Content" };
            var userId = "unauthorizedUserId";
            var existingPost = new Post { Id = postId, Title = "Original Post", Content = "Original Content", UserId = "authorizedUserId" };
            _mockPostService.Setup(s => s.GetPostById(postId)).ReturnsAsync(existingPost);

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
            var result = await _controller.UpdatePost(postId, postDto);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task DeletePost_ValidId_ReturnsOk()
        {
            // Arrange
            var postId = 1;

            // Act
            var result = await _controller.DeletePost(postId);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
