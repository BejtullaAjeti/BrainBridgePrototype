using AutoMapper;
using BrainBridgePrototype.DTOs;
using BrainBridgePrototype.Models;
using BrainBridgePrototype.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrainBridgePrototype.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(int postId)
        {
            var comments = await _commentService.GetCommentsByPostId(postId);
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await _commentService.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpPost("post/{postId}")]
        public async Task<IActionResult> CreateComment(int postId, CommentDto commentDto)
        {
            try
            {
                if (string.IsNullOrEmpty(commentDto.UserId))
                {
                    return BadRequest("UserId is required.");
                }

                var comment = await _commentService.CreateComment(commentDto, commentDto.UserId, postId);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentDto commentDto)
        {
            try
            {
                // Retrieve comment from service
                var comment = await _commentService.GetCommentById(id);

                // Check if comment exists
                if (comment == null)
                {
                    return NotFound();
                }

                // Check if the user is authorized to update the comment
                if (commentDto.UserId != comment.UserId && !User.IsInRole("Admin"))
                {
                    return Forbid(); // Return ForbidResult if user is not authorized
                }

                // Update comment
                var updatedComment = await _commentService.UpdateComment(id, commentDto, commentDto.UserId);

                // Return updated comment
                return Ok(updatedComment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }





        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentService.DeleteComment(id);
            return Ok();
        }
    }
}
