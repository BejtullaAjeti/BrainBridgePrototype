using AutoMapper;
using BrainBridgePrototype.DTOs;
using BrainBridgePrototype.Models;
using BrainBridgePrototype.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BrainBridgePrototype.Services;

namespace BrainBridgePrototype.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postService.GetAllPosts();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostDto postDto)
        {
            try
            {
                if (string.IsNullOrEmpty(postDto.UserId))
                {
                    return BadRequest("UserId is required.");
                }

                var post = await _postService.CreatePost(postDto, postDto.UserId);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, PostDto postDto)
        {
            try
            {
                if (string.IsNullOrEmpty(postDto.UserId))
                {
                    return BadRequest("UserId is required.");
                }

                var post = await _postService.GetPostById(id);

                if (post == null)
                {
                    return NotFound();
                }

                if (postDto.UserId != post.UserId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var updatedPost = await _postService.UpdatePost(id, postDto, postDto.UserId);
                return Ok(updatedPost);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }





        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.DeletePost(id);
            return Ok();
        }

    }
}
