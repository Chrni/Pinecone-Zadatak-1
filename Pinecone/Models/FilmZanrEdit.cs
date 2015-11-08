using System.Collections.Generic;

namespace Pinecone.Models
{
    public class FilmZanrEdit
    {
        public int Id_film { get; set; }
        public List<ZanrDTO> Zanr { get; set; }
    }
}