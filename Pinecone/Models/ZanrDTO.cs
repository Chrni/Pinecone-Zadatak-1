using System.ComponentModel.DataAnnotations;

namespace Pinecone.Models
{
    public class ZanrDTO
    {
        public int Id { get; set; }

        [Required]
        public string Zanr { get; set; }
    }
}