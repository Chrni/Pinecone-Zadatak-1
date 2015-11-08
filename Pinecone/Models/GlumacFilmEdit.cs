using System.Collections.Generic;

namespace Pinecone.Models
{
    public class GlumacFilmEdit
    {
        public int Id_film { get; set; }
        public List<GlumacDTO> Glumac { get; set; }
    }
}