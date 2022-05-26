using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ChatWebAPI.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; }
        public bool Sent { get; set; }
        public DateTime Created { get; set; }
        [JsonIgnore]
        public Contact? Contact { get; set; }

    }
}