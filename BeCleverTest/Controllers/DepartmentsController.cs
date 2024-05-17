using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeCleverTest.Models;

namespace BeCleverTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        // obtener todos los departments
        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            try
            {
                
                var Departments  = await _context.Departments.ToListAsync();

                return Ok(Departments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error Al Intentar Obtener Department!", error = ex.Message });
            }
        }

        //obtener un department por su id
        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            try{
                var Department = await _context.Departments.FindAsync(id);

                if (Department == null)
                {
                    return NotFound(new { message = "Department no encontrado" });
                }

                return Ok(Department);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error Al Intentar Obtener Department!", error = ex.Message });
            }
        }

        // actualizar un department existente
        // PUT: api/Departments/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutDepartment(int id, Department department)
        {
            try
            {
                if (id != department.IdDepartment)
                {
                    return BadRequest(new { message = "El Id Del Department No Coincide!" });
                }

                _context.Entry(department).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok(new { message = "El Department Con El Id " + id + " Se Actualizo." });
            }catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound(new { message = "El Department Que Intentas Actualizar No Existe!" });
                }
                else
                {
                    throw;
                }
            }
        }

        // crear un nuevo department
        // POST: api/Departments/new
        [HttpPost("new")]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {
            try
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetDepartment", new { id = department.IdDepartment }, department);
            }catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error En La Creacion Del Department!", error = ex.Message });
            }
            
        }

        //eliminar un department por su id
        // DELETE: api/Departments/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                {
                    return NotFound(new { message = "El Department Que Intentas Eliminar No Existe!" });
                }

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();

                return Ok(new { message = "El Department Se Elimino Correctamente!" });
            }catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error En La Eliminacion Del Department!", error = ex.Message });
            }
            
        }

        // metodo para comprobar si un department existe en la base de datos
        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.IdDepartment == id);
        }
    }
}
