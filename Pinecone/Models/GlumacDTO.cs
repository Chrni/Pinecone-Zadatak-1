using System.ComponentModel.DataAnnotations;

namespace Pinecone.Models
{
    public class GlumacDTO
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Ime { get; set; }
    }
}