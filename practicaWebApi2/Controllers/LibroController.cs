using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using practicaWebApi2.Models;
using Microsoft.EntityFrameworkCore;

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
            var LibroAutor = (from Libro in _bibliotecaContext.Libro
                              join Autor in _bibliotecaContext.Autor on Libro.id_libro equals Autor.id_autor
                              where Libro.id_libro == id
                              select new
                              {                                  
                                  Libro = Libro.titulo,
                                  Autor.nombre
                              }).ToList();

            if (LibroAutor == null)
            {
                return NotFound();
            }
            return Ok(LibroAutor);
        }


        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarLibro([FromBody] Libro libro)
        {
            try
            {
                _bibliotecaContext.Libro.Add(libro);
                _bibliotecaContext.SaveChanges();
                return Ok(libro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] Libro libroModificar)
        {
            Libro? libroActual = (from Libro in _bibliotecaContext.Libro
                                  where Libro.id_libro == id 
                                  select Libro).FirstOrDefault();
                        
            if (libroActual == null) 
            {
                return NotFound();
            }

            libroActual.titulo = libroModificar.titulo; 
            libroActual.anyo_publicacion = libroModificar.anyo_publicacion;
            libroActual.resumen = libroModificar.resumen;

            _bibliotecaContext.Entry(libroActual).State = EntityState.Modified; 
            _bibliotecaContext.SaveChanges();

            return Ok(libroModificar);
        }


        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarLibro(int id)
        {            
            Libro? libro = (from Libro in _bibliotecaContext.Libro
                            where Libro.id_libro == id 
                            select Libro).FirstOrDefault();

            if (libro == null) 
            {
                return NotFound();
            }

            _bibliotecaContext.Libro.Attach(libro);
            _bibliotecaContext.Libro.Remove(libro);
            _bibliotecaContext.SaveChanges(); 

            return Ok(libro);
        }


        [HttpPut]
        [Route("buscarAfter2000")]
        public IActionResult Buscar ()
        {
            var librosEncontrados = (from Libro in _bibliotecaContext.Libro
                                     where Libro.anyo_publicacion>2000
                                     select Libro).ToList();
            return Ok(librosEncontrados);
        }


        [HttpGet]
        [Route("GetPaginado")]
        public IActionResult GetPaginado()
        {
            List<Libro> listadoLibro = (from Libro in _bibliotecaContext.Libro select Libro).Skip(10).ToList();


            if (listadoLibro.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoLibro);
        }


        [HttpGet]
        [Route("GetByTitulo")]
        public IActionResult GetByTitulo(string titulo)
        {
            var listadoLibro = (from Libro in _bibliotecaContext.Libro
                                where Libro.titulo.Contains(titulo) //constains es el like '%---%'
                                select Libro.titulo).ToList();

            if (listadoLibro.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoLibro);
        }
    }
}
