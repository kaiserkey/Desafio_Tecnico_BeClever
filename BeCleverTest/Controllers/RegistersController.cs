using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeCleverTest.Models;
using FluentValidation;

namespace BeCleverTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RegistersController(AppDbContext context)
        {
            _context = context;
        }

        private bool BeValidDateTime(DateTime? dateTime)
        {
            // Comprueba si el DateTime no es nulo
            if (dateTime.HasValue)
            {
                // Comprueba si la fecha es válida y si la hora es diferente de la medianoche
                return dateTime.Value != DateTime.MinValue && dateTime.Value.TimeOfDay != TimeSpan.Zero;
            }
            return false; // Retorna false si el DateTime es nulo
        }

        private bool BeValidDateTimeNow(DateTime? dateTime)
        {
            DateTime today = DateTime.Today;
            return dateTime >= today; // Retorna false si la fecha del registro no es mayor o igual a la fecha actual
        }

        /***
         * Services 1
         * Generar los servicios correspondientes, los cuales nos permitan guardar en base
         * de datos, los ingresos y egresos del personal.
         * Function register(idEmployee, date, registerType, businessLocation) 
         */
        /**
         * Hay muchas cuestiones a tener en cuenta a la hora de insertar un nuevo register en la base de datos,
         * aqui intente capturar las que segun mi criterio son las mas importantes y aun asi se me deben haber pasado
         * muchas de ellas.
         */
        // crear un nuevo Register en la base de datos
        // POST: api/Registers/Register
        [HttpPost("Register")]
        public async Task<ActionResult<Register>> PostRegister(Register register)
        {
            //validaciones de cada campo del register antes de intentar insertarlos en la base de datos
            var validator = new InlineValidator<Register>();

            validator.RuleFor(x => x.IdEmployee).NotEmpty().WithMessage("El Id del empleado es obligatorio.");
            validator.RuleFor(x => x.IdBusiness).NotEmpty().WithMessage("El Id de la sucursal es obligatorio.");
            validator.RuleFor(x => x.DateTime)
                     .NotEmpty().WithMessage("La fecha y hora son obligatorias.")
                     .Must(BeValidDateTime).WithMessage("La fecha y hora deben ser válidas.")
                     .Must(BeValidDateTimeNow).WithMessage("La fecha y hora deben ser mayor o igual a la fecha y hora actual.");

            validator.RuleFor(x => x.RegisterType)
                     .NotEmpty().WithMessage("El tipo de registro es obligatorio.")
                     .Must(type => type == "ingreso" || type == "egreso")
                     .WithMessage("El tipo de registro debe ser 'ingreso' o 'egreso'.");

            var validationResult = await validator.ValidateAsync(register);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            //validamos que el IdBusiness sea un id valido
            BusinessesController business = new BusinessesController(this._context);
            var businessExists = business.BusinessExists(register.IdBusiness);

            if (!businessExists)
            {
                return BadRequest(new { message = "El IdBusiness Ingresado No Es Valido!" });
            }

            //validamos que el IdEmployee sea un id valido
            EmployeesController employee = new EmployeesController(this._context);
            var employeeExists = employee.EmployeeExists(register.IdEmployee);

            if (!employeeExists)
            {
                return BadRequest(new { message = "El IdEmployee Ingresado No Es Valido!" });
            }


            //busca los registros del empleado en la empresa seleccionada el dia actual
            DateTime today = DateTime.Today;

            var ultimoRegistro = _context.Registers.Where(r => r.DateTime >= today && r.DateTime < today.AddDays(1) && r.IdEmployee == register.IdEmployee && r.IdBusiness == register.IdBusiness)  // Filtrar registros para la fecha de hoy
                                                    .OrderByDescending(r => r.IdRegister)
                                                    .FirstOrDefault();

            // realizacion de todos los controles posibles para que los ingresos y egresos sean correctos
            //si la persona no tiene ningun registro para el dia actual y marco un 'egreso' mostramos un error de que debe marcar un 'ingreso'
            if (ultimoRegistro == null && register.RegisterType.Equals("egreso"))
            {
                return BadRequest(new { message = "Usted No Tiene Ningun 'Ingreso' o 'Egreso' El Dia De Hoy Por Lo Tanto Debe Ingresar Como 'Ingreso' Al Sistema!" });
            }

            //si se encontro un registro para el dia actual y ese registro es un 'ingreso' pero en el RegisterType ha puesto 'ingreso' entonces mandamos un error
            if (ultimoRegistro != null && ultimoRegistro.RegisterType.Equals("ingreso") && register.RegisterType.Equals("ingreso"))
            {
                return BadRequest(new { message = "El Ultimo Registro Ingresado Es Un 'Ingreso' Por Lo Que El Actual Registro Debe Ser 'Egreso'!" });
            }

            //si se encontro un registro para el dia actual y ese registro es un 'egreso' pero en el RegisterType ha puesto 'egreso' entonces mandamos un error
            if (ultimoRegistro != null && ultimoRegistro.RegisterType.Equals("egreso") && register.RegisterType.Equals("egreso"))
            {
                return BadRequest(new { message = "El Ultimo Registro Ingresado Es Un 'Egreso' Por Lo Que El Actual Registro Debe Ser 'Ingreso'!" });
            }

            //si se encontro un registro para el dia actual y ese registro es un 'ingreso' pero en el RegisterType ha puesto 'ingreso' entonces mandamos un error
            if (ultimoRegistro != null && ultimoRegistro.RegisterType.Equals("ingreso") && register.RegisterType.Equals("ingreso"))
            {
                return BadRequest(new { message = "El Ultimo Registro Ingresado Es Un 'Ingreso' Por Lo Que El Actual Registro Debe Ser 'Egreso'!" });
            }

            // Si la fecha del registro actual es menor o igual a la fecha del ultimo ingreso registrado,
            // devuelve un mensaje de error indicando que la fecha y hora deben ser posteriores al ultimo ingreso.
            if (ultimoRegistro != null && ultimoRegistro.RegisterType.Equals("ingreso"))
            {
                if (register.DateTime <= ultimoRegistro.DateTime)
                {
                    return BadRequest(new { message = "La Fecha Y Hora Del 'Egreso' Deben Ser Posteriores Al Ultimo 'Ingreso': " + ultimoRegistro.DateTime });
                }
            }

            //mismo caso que el anterior pero para los egresos
            if (ultimoRegistro != null && ultimoRegistro.RegisterType.Equals("egreso"))
            {
                if (register.DateTime <= ultimoRegistro.DateTime)
                {
                    return BadRequest(new { message = "La Fecha Y Hora Del 'Ingreso' Deben Ser Posteriores Al Ultimo 'Egreso': " + ultimoRegistro.DateTime });
                }
            }

            //si el register pasa todas las verificaciones entonces lo insertamos en la base de datos

            try
            {
                _context.Registers.Add(register);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetRegister", new { id = register.IdRegister }, register);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error En La Creacion Del Register!", error = ex.Message });
            }

        }

        //clase para capturar las validaciones con FluentValidation
        public class SearchQuery
        {
            public DateTime? DateFrom { get; set; }
            public DateTime? DateTo { get; set; }
            public string? DescriptionFilter { get; set; }
            public int? BusinessLocation { get; set; }
        }

        /***
         * Services 2
         * Generar un servicio el cual liste la cantidad de ingresos y egresos dada una fecha
         * desde – hasta, que se pueda filtrar por nombre o apellido y sucursal.
         * Function search(dateFrom, dateTo , descriptionFilter,
         * businessLocation)
         */
        // GET: api/Registers/Search
        [HttpGet("Search")]
        public async Task<ActionResult> Search([FromForm] SearchQuery searchquery)
        {
            // Validaciones
            var validator = new InlineValidator<SearchQuery>();

            validator.RuleFor(x => x.DateFrom).NotEmpty().WithMessage("La fecha inicial es obligatoria.");
            validator.RuleFor(x => x.DateTo).NotEmpty().WithMessage("La fecha final es obligatoria.")
                                            .GreaterThan(x => x.DateFrom).WithMessage("La fecha final debe ser mayor que la fecha inicial.");
            validator.RuleFor(x => x.DescriptionFilter).NotEmpty().WithMessage("El DescriptionFilter es obligatorio.");

            var validationResult = await validator.ValidateAsync(searchquery);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            //validamos que el BusinessLocation sea un id valido
            BusinessesController business = new BusinessesController(this._context);
            var businessExists = business.BusinessExists(searchquery.BusinessLocation);

            if (!businessExists)
            {
                return BadRequest(new { message = "El BusinessLocation Ingresado No Es Valido, Debe Ser Un Id Numerico!" });
            }

            try
            {

                //primero aplicamos el filtro por la fecha desde y hasta
                IQueryable<Register> query = _context.Registers.Include(r => r.IdEmployeeNavigation)
                                                                .Include(r => r.IdBusinessNavigation)
                                                                .Where(r => r.DateTime >= searchquery.DateFrom && r.DateTime <= searchquery.DateTo);

                // ahora aplicamos los filtros opcionales solo si pasan las validaciones
                if (!string.IsNullOrEmpty(searchquery.DescriptionFilter))
                {
                    query = query.Where(r => r.IdEmployeeNavigation.FirstName.Contains(searchquery.DescriptionFilter) ||
                                             r.IdEmployeeNavigation.LastName.Contains(searchquery.DescriptionFilter));
                }

                if (searchquery.BusinessLocation.HasValue)
                {
                    query = query.Where(r => r.IdBusinessNavigation.IdBusiness == searchquery.BusinessLocation);
                }

                var registros = await query.ToListAsync();

                // generamos un json con los resultados
                var jsonObject = new
                {
                    empleado = registros.FirstOrDefault().IdEmployeeNavigation.FirstName + ' ' + registros.FirstOrDefault().IdEmployeeNavigation.LastName,//mostrar el nombre del empleado
                    totalDeRegistros = registros.Count, // contar el total de registros
                    totalIngresos = registros.Where(r => r.RegisterType.Equals("ingreso")).Count(), //contar ingresos
                    totalSalidas = registros.Where(r => r.RegisterType.Equals("egreso")).Count(), //contar egresos
                    listaDeRegistros = registros.Select(r => new //listar todos los registros encontrados, estos podrian ser demasiados y se tendrian que limitar o agregar mas filtros
                    {
                        idRegister = r.IdRegister,
                        idEmployee = r.IdEmployee,
                        dateTime = r.DateTime,
                        registerType = r.RegisterType,
                        idBusiness = r.IdBusiness
                    })
                };

                return Ok(jsonObject);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error En La Busqueda Del Register!", error = ex.Message });
            }
        }

        /**
         * Services 3
         * Generar un servicio el cual devuelva el promedio de hombres y mujeres que
         * ingresan y egresan por mes, por cada sucursal.
         * Function average (dateFrom, dateTo )          
         */
        // GET: api/Registers/Average
        [HttpGet("Average")]
        public async Task<ActionResult> Average([FromForm] SearchQuery searchquery)
        {
            // Verificar si dateFrom y dateTo tienen valores y son fechas validas
            var validator = new InlineValidator<SearchQuery>();

            validator.RuleFor(x => x.DateFrom).NotEmpty().WithMessage("La fecha inicial es obligatoria.");
            validator.RuleFor(x => x.DateTo).NotEmpty().WithMessage("La fecha final es obligatoria.")
                                            .GreaterThan(x => x.DateFrom).WithMessage("La fecha final debe ser mayor que la fecha inicial.");

            var validationResult = await validator.ValidateAsync(searchquery);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                // calculamos la diferencia de meses entre dateFrom y dateTo para luego porder calcular el promedio segun la cantidad de meses que hay en el periodo ingresado
                var totalMeses = ((searchquery.DateTo.Value.Year - searchquery.DateFrom.Value.Year) * 12) + searchquery.DateTo.Value.Month - searchquery.DateFrom.Value.Month + 1;

                // agrupamos los registros por sucursal, año, mes, género (masculino/femenino) y tipo de registro (ingreso/egreso)
                var registros = await _context.Registers.Include(r => r.IdBusinessNavigation)
                                                        .Include(r => r.IdEmployeeNavigation)
                                                        .Where(r => r.DateTime >= searchquery.DateFrom && r.DateTime <= searchquery.DateTo)
                                                        .GroupBy(r => new { r.IdBusinessNavigation.LocationName, 
                                                                            Year = r.DateTime.Value.Year, 
                                                                            Month = r.DateTime.Value.Month, 
                                                                            r.IdEmployeeNavigation.Gender, 
                                                                            r.RegisterType })
                                                        .Select(g => new
                                                        {
                                                            Sucursal = g.Key.LocationName,
                                                            Año = g.Key.Year,
                                                            Mes = g.Key.Month,
                                                            Genero = g.Key.Gender,
                                                            TipoRegistro = g.Key.RegisterType,
                                                            Cantidad = g.Count()
                                                        })
                                                        .ToListAsync();

                // por ultimo sacamos el promedio por cada registro obtenido
                var promedios = registros
                    .GroupBy(r => new { r.Sucursal, 
                                        r.Año, 
                                        r.Mes,
                                        r.Genero, 
                                        r.TipoRegistro })
                    .Select(g => new
                    {
                        Sucursal = g.Key.Sucursal,
                        Fecha = $"{g.Key.Mes:D2}/{g.Key.Año}", // Formato MM/YYYY
                        Genero = g.Key.Genero,
                        TipoRegistro = g.Key.TipoRegistro,
                        PromedioMensual = g.Sum(r => r.Cantidad) / (double)totalMeses
                    })
                    .ToList();

                return Ok(promedios);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error Al Calcular El Promedio!", error = ex.Message });
            }
        }


        //obtener todos los Registers
        // GET: api/Registers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Register>>> GetRegisters()
        {
            try
            {
                var Registers =  await _context.Registers.ToListAsync();

                return Ok(Registers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error Al Intentar Obtener Los Registers!", error = ex.Message });
            }
        }

        //obtener un Register por su id
        // GET: api/Registers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Register>> GetRegister(int id)
        {
            try
            {
                var Register = await _context.Registers.FindAsync(id);

                if (Register == null)
                {
                    return NotFound(new { message = "Register no encontrado" });
                }

                return Ok(Register);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error Al Intentar Obtener El Register!", error = ex.Message });
            }
        }

        //actualizar un Register por su id
        // PUT: api/Registers/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutRegister(int id, Register register)
        {
            try
            {
                if (id != register.IdRegister)
                {
                    return BadRequest(new { message = "El Id De Register No Coincide!" });
                }

                _context.Entry(register).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok(new { message = "El Register Con El Id " + id + " Se Actualizo." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegisterExists(id))
                {
                    return NotFound(new { message = "El Register Que Intentas Actualizar No Existe!" });
                }
                else
                {
                    throw;
                }
            }
        }


        // eliminar un Register por su id
        // DELETE: api/Registers/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRegister(int id)
        {
            try
            {
                var register = await _context.Registers.FindAsync(id);
                if (register == null)
                {
                    return NotFound(new { message = "El Register Que Intentas Eliminar No Existe!" });
                }

                _context.Registers.Remove(register);
                await _context.SaveChangesAsync();

                return Ok(new { message = "El Register Se Elimino Correctamente!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrio Un Error En La Eliminacion Del Register!", error = ex.Message });
            }
        }

        private bool RegisterExists(int id)
        {
            return _context.Registers.Any(e => e.IdRegister == id);
        }

    }
}
