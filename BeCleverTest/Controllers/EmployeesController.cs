using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeCleverTest.Models;

namespace BeCleverTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        //obtener todos los employees
        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            try
            {
                var Employees =  await _context.Employees.ToListAsync();

                return Ok(Employees);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error Al Intentar Obtener Employees!", error = ex.Message });
            }
        }

        //obtener un Employee por su id
        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var Employee = await _context.Employees.FindAsync(id);

                if (Employee == null)
                {
                    return NotFound(new { message = "Employee no encontrado" });
                }

                return Ok(Employee);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error Al Intentar Obtener Employee!", error = ex.Message });
            }
        }

        //actualizar un employee existente
        // PUT: api/Employees/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {

            try
            {
                if (id != employee.IdEmployee)
                {
                    return BadRequest(new { message = "El Id Del Employee No Coincide!" });
                }

                _context.Entry(employee).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok(new { message = "El Employee Con El Id " + id + " Se Actualizo." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound(new { message = "El Employee Que Intentas Actualizar No Existe!" });
                }
                else
                {
                    throw;
                }
            }
        }

        //crear un nuevo employee
        // POST: api/Employees/new
        [HttpPost("new")]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            try
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEmployee", new { id = employee.IdEmployee }, employee);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error En La Creacion Del Employee!", error = ex.Message });
            }
        }

        //eliminar un employee por su id
        // DELETE: api/Employees/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound(new { message = "El Employee Que Intentas Eliminar No Existe!" });
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return Ok(new { message = "El Employee Se Elimino Correctamente!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error En La Eliminacion Del Employee!", error = ex.Message });
            }
        }

        // metodo para comprobar si un employee existe en la base de datos
        [NonAction]
        public bool EmployeeExists(int? id)
        {
            return _context.Employees.Any(e => e.IdEmployee == id);
        }
    }
}
