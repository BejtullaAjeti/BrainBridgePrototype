using System.Text.Json.Serialization;

namespace BrainBridgePrototype.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int PostId { get; set; }


        [JsonIgnore]
        public Post Post { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

    }
}
