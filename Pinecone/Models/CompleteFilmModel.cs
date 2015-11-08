using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pinecone.Models
{
    public class CompleteFilmModel
    {

        public int Id_Film { get; set; }

        [Required, StringLength(70)]
        public string Naslov { get; set; }

        [Required, StringLength(100)]
        public string Radnja { get; set; }

        [Required, Range(1, 1000)]
        public int Trajanje { get; set; }

        [Required, Range(1, 10)]
        public decimal Ocjena { get; set; }
        public List<ZanrDTO> Zanr { get; set; }
        public List<GlumacDTO> Glumci { get; set; }
    }
}