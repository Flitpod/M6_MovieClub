using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M6_MovieClub.Models
{
    public class Movie
    {
        [Key]
        public string Uid { get; set; }
        public string Title { get; set; }
        public string OwnerId { get; set; }

        // attach owner user object with lazy loading, but not map this into database
        [NotMapped]
        public virtual SiteUser Owner { get; set; }

        public Movie()
        {
            this.Uid = Guid.NewGuid().ToString();
        }
    }
}
