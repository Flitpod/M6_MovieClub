using System.ComponentModel.DataAnnotations;

namespace M6_MovieClub.Models
{
    public class Movie
    {
        [Key]
        public string Uid { get; set; }
        public string Title { get; set; }
        public string OwnerId { get; set; }

        public Movie()
        {
            this.Uid = Guid.NewGuid().ToString();
        }
    }
}
