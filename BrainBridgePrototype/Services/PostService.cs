using AutoMapper;
using BrainBridgePrototype.DTOs;
using BrainBridgePrototype.Models;
using BrainBridgePrototype.Repositories;

namespace BrainBridgePrototype.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IMapper _mapper;

        public PostService(IRepository<Post> postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _postRepository.GetAll();
        }

        public async Task<Post> GetPostById(int id)
        {
            return await _postRepository.GetById(id);
        }

        public async Task<Post> CreatePost(PostDto postDto, string userId)
        {
            var post = _mapper.Map<Post>(postDto);
            post.UserId = userId;
            post.CreatedAt = DateTime.UtcNow;
            await _postRepository.Add(post);
            return post;
        }

        public async Task<Post> UpdatePost(int id, PostDto postDto, string userId)
        {
            var post = await _postRepository.GetById(id);
            if (post == null || post.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to edit this post.");
            }
            _mapper.Map(postDto, post);
            post.UpdatedAt = DateTime.UtcNow;
            await _postRepository.Update(post);
            return post;
        }

        public async Task DeletePost(int id)
        {
            await _postRepository.Delete(id);
        }
    }
}
