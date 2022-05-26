using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChatWebAPI.Models
{
    public class Contact
    {
        [Key]
        [JsonIgnore]
        public int ContactId { get; set; }
        public string Id { get; set; } 
        public string? Name { get; set; }
        public string? Last { get; set; }
        public string? Server { get; set; }
        public DateTime LastDate{ get; set; }
        [JsonIgnore]

        public int UserId { get; set; } 
        public virtual ICollection<Message>? Messages { get; set; }
        [JsonIgnore]
        public User? User { get; set; } 

    }
}
