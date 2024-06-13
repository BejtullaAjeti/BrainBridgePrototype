using BrainBridgePrototype.DTOs;
using BrainBridgePrototype.Models;

namespace BrainBridgePrototype.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetCommentsByPostId(int postId);
        Task<Comment> GetCommentById(int id);
        Task<Comment> CreateComment(CommentDto commentDto, string userId, int postId);
        Task<Comment> UpdateComment(int id, CommentDto commentDto, string userId);
        Task DeleteComment(int id);
    }
}
