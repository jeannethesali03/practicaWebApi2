using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using practicaWebApi2.Models;
using Microsoft.EntityFrameworkCore;

namespace practicaWebApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly BibliotecaContext _bibliotecaContext;
        public AutorController(BibliotecaContext bibliotecaContexto)
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
            List<Autor> listadoAutor = (from Autor in _bibliotecaContext.Autor select Autor).ToList();

            if (listadoAutor.Count == 0) 
            {
                return NotFound();
            }

            return Ok(listadoAutor);
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
            var AutorLibro = (from Autor in _bibliotecaContext.Autor
                            join Libro in _bibliotecaContext.Libro on Autor.id_autor equals Libro.id_autor
                                where Autor.id_autor == id
                                select new
                                {
                                    Autor.nombre,
                                    Libro = Libro.titulo
                                }).ToList();

            if (AutorLibro == null)
            {
                return NotFound();
            }
            return Ok(AutorLibro);
        }


        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] Autor autor)
        {
            try
            {
                _bibliotecaContext.Autor.Add(autor);
                _bibliotecaContext.SaveChanges();
                return Ok(autor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] Autor autorModificar)
        {
            // Para actualizar un registro, se obtiene el registro original de la base de datos
            // al cual alteraremos alguna propiedad
            Autor? autorActual = (from Autor in _bibliotecaContext.Autor
                                     where Autor.id_autor == id // Corregido: == en lugar de =
                                     select Autor).FirstOrDefault();

            // Verificamos que exista el registro segun su ID
            if (autorActual == null) // Corregido: == en lugar de =
            {
                return NotFound();
            }

            // Si se encuentra el registro, se alteran los campos modificables
            autorActual.nombre = autorModificar.nombre; // Corregido: asignación con =
            autorActual.nacionalidad = autorModificar.nacionalidad; // Corregido: asignación con =
            

            // Se marca el registro como modificado en el contexto
            // y se envia la modificacion a la base de datos
            _bibliotecaContext.Entry(autorActual).State = EntityState.Modified; // Corregido: _equiposContexto
            _bibliotecaContext.SaveChanges(); // Corregido: _equiposContexto

            return Ok(autorModificar);
        }


        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            // Para eliminar un registro, se obtiene el registro original de la base de datos
            // al cual eliminaremos
            Autor? autor = (from Autor in _bibliotecaContext.Autor // _equiposContexto corregido
                               where Autor.id_autor == id // == para comparación, no = para asignación
                               select Autor).FirstOrDefault();

            // Verificamos que exista el registro según su ID
            if (autor == null) // == para comparación, no = para asignación
            {
                return NotFound();
            }

            // Ejecutamos la acción de eliminar el registro
            _bibliotecaContext.Autor.Attach(autor); // _equiposContexto corregido
            _bibliotecaContext.Autor.Remove(autor); // _equiposContexto corregido
            _bibliotecaContext.SaveChanges(); // _equiposContexto corregido

            return Ok(autor);
        }
    }
}
