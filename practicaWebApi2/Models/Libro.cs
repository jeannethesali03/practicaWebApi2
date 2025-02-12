using System.ComponentModel.DataAnnotations;

namespace practicaWebApi2.Models
{
    public class Libro
    {
        [Key]
        public int id_libro { get; set; }
        public string titulo { get; set; }
        public int anyo_publicacion { get; set; }
        public int id_autor { get; set; }
        public int id_categoria { get; set; }
        public string resumen { get; set; }



    }
}
