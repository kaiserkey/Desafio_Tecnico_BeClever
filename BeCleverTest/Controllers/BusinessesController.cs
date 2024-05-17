using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeCleverTest.Models;

namespace BeCleverTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BusinessesController(AppDbContext context)
        {
            _context = context;
        }

        //obtener todas las Businesses
        // GET: api/Businesses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Business>>> GetBusinesses()
        {
            
            try
            {
                var Businesses = await _context.Businesses.ToListAsync();

                return Ok(Businesses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error Al Intentar Obtener Businesses!", error = ex.Message });
            }
        }

        // obtener la Business por su id
        // GET: api/Businesses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Business>> GetBusiness(int id)
        {
            try
            {
                var Business = await _context.Businesses.FindAsync(id);

                if (Business == null)
                {
                    return NotFound(new { message = "Business no encontrado" });
                }

                return Ok(Business);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error Al Intentar Obtener Business!", error = ex.Message });
            }
        }

        //actualzar un Business por su id
        // PUT: api/Businesses/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutBusiness(int id, Business business)
        {
            try
            {
                if (id != business.IdBusiness)
                {
                    return BadRequest(new { message = "El Id De Business No Coincide!" });
                }

                _context.Entry(business).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Business Con El Id " + id + " Se Actualizo." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessExists(id))
                {
                    return NotFound(new { message = "El Business Que Intentas Actualizar No Existe!" });
                }
                else
                {
                    throw;
                }
            }
        }

        // crear un nuevo business en la base de datos
        // POST: api/Businesses/new
        [HttpPost("new")]
        public async Task<ActionResult<Business>> PostBusiness(Business business)
        {
            try
            {
                _context.Businesses.Add(business);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBusiness", new { id = business.IdBusiness }, business);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error En La Creacion De Business!", error = ex.Message });
            }
        }

        //elimina un business por su id
        // DELETE: api/Businesses/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBusiness(int id)
        {
            try
            {
                var business = await _context.Businesses.FindAsync(id);
                if (business == null)
                {
                    return NotFound(new { message = "La Business Que Intentas Eliminar No Existe!" });
                }

                _context.Businesses.Remove(business);
                await _context.SaveChangesAsync();

                return Ok(new { message = "La Business Se Elimino Correctamente!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error En La Eliminacion De Business!", error = ex.Message });
            }
        }

        // metodo para comprobar si un business existe en la base de datos
        [NonAction]
        public bool BusinessExists(int? id)
        {
            return _context.Businesses.Any(e => e.IdBusiness == id);
        }
    }
}
