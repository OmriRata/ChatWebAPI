using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChatWebAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required,MinLength(3)]
        public string? UserName { get; set; }
        [Required, MinLength(3)]
        public string? NickName { get; set; }
        [Required, MinLength(3),DataType(DataType.Password)]
        public string? Password { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Contact>? Contacts { get; set; } = new List<Contact>();  



    }
}
