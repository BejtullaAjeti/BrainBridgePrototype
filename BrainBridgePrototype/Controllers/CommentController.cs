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
    [Authorize]
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var comment = await _commentService.CreateComment(commentDto, userId, postId);
            return Ok(comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentDto commentDto)
        {
            // Retrieve comment from service
            var comment = await _commentService.GetCommentById(id);

            // Check if comment exists
            if (comment == null)
            {
                return NotFound();
            }

            // Check if the user is authorized to update the comment
            if (comment.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid(); // Return ForbidResult if user is not authorized
            }

            // Update comment
            var updatedComment = await _commentService.UpdateComment(id, commentDto, comment.UserId);

            // Return updated comment
            return Ok(updatedComment);
        }



        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentService.DeleteComment(id);
            return Ok();
        }
    }
}
