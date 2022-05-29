using System.ComponentModel.DataAnnotations;

namespace ChatWebAPI.Models
{
    public class Rating
    {
        [Key]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Rate { get; set; }
    }
}
