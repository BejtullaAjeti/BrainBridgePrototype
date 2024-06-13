using AutoMapper;
using BrainBridgePrototype.DTOs;
using BrainBridgePrototype.Models;
using BrainBridgePrototype.Repositories;

namespace BrainBridgePrototype.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<Post> _postRepository;
        private readonly IMapper _mapper;

        public CommentService(IRepository<Comment> commentRepository, IRepository<Post> postRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId)
        {
            return await _commentRepository.Find(c => c.PostId == postId);
        }

        public async Task<Comment> GetCommentById(int id)
        {
            return await _commentRepository.GetById(id);
        }

        public async Task<Comment> CreateComment(CommentDto commentDto, string userId, int postId)
        {
            var post = await _postRepository.GetById(postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post not found.");
            }

            var comment = _mapper.Map<Comment>(commentDto);
            comment.UserId = userId;
            comment.PostId = postId;
            comment.CreatedAt = DateTime.UtcNow;
            await _commentRepository.Add(comment);
            return comment;
        }

        public async Task<Comment> UpdateComment(int id, CommentDto commentDto, string userId)
        {
            var comment = await _commentRepository.GetById(id);
            if (comment == null || comment.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to edit this comment.");
            }
            _mapper.Map(commentDto, comment);
            comment.UpdatedAt = DateTime.UtcNow;
            await _commentRepository.Update(comment);
            return comment;
        }

        public async Task DeleteComment(int id)
        {
            await _commentRepository.Delete(id);
        }
    }
}
