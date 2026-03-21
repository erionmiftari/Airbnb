namespace Airbnb.Models
{
    public class Listing
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int OwnerId { get; set; }
    }
}