using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using practicaWebApi2.Models;

namespace practicaWebApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly BibliotecaContext _bibliotecaContext;
        public LibroController(BibliotecaContext bibliotecaContexto)
        {
            _bibliotecaContext = bibliotecaContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos los equipos existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Libro> listadoLibro = (from Libro in _bibliotecaContext.Libro select Libro).ToList();

            if (listadoLibro.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoLibro);
        }


        /// <summary>
        /// EndPoint que retorna los registros de una tabla fitrados por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            var LibroAutor = (from Autor in _bibliotecaContext.Autor
                              join Libro in _bibliotecaContext.Libro on Autor.id_autor equals Libro.id_autor
                              where Autor.id_autor == id
                              select new
                              {
                                  Autor.nombre,
                                  Libro = Libro.titulo
                              }).ToList();

            if (LibroAutor == null)
            {
                return NotFound();
            }
            return Ok(LibroAutor);
        }
    }
}
