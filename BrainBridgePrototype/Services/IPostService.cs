using BrainBridgePrototype.DTOs;
using BrainBridgePrototype.Models;

namespace BrainBridgePrototype.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPosts();
        Task<Post> GetPostById(int id);
        Task<Post> CreatePost(PostDto postDto, string userId);
        Task<Post> UpdatePost(int id, PostDto postDto, string userId);
        Task DeletePost(int id);
    }
}
