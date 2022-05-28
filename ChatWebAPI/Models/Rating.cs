namespace ChatWebAPI.Models
{
    public class Rating
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Rate { get; set; }
        List<String>? RatingList { get; set; }
    }
}
