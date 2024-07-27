using InfoDevelopers.Data;
using InfoDevelopers.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InfoDevelopers.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpGet("GetQualification")]
        public IActionResult GetQualification()
        {
            var result = _context.Qualifications.ToList();
            return Ok(result);
        }


         //POST: api/Employee/add
        [HttpPost("add")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = employeeDto.Name,
                DOB = employeeDto.DOB,
                Gender = employeeDto.Gender,
                Salary = employeeDto.Salary,
                EntryBy = "User", 
                EntryDate = DateTime.UtcNow,
            };
            employee.EmployeeQualifications = employeeDto.Qualifications.Select(q => new EmployeeQualification
            {
                Marks = q.Marks,
                QualificationId = q.QualificationId,
                EmployeeId = employee.Id
            }).ToList();

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpGet("GetEmployees")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _context.Employees
                                          .Include(e => e.EmployeeQualifications)
                                          .ThenInclude(eq => eq.Qualification)
                                          .ToListAsync();

            if (employees == null)
            {
                return NotFound();
            }

            return Ok(employees);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class EmployeeDto
    {
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public double? Salary { get; set; }
        public List<QualificationDto> Qualifications { get; set; }
    }

    public class QualificationDto
    {
        public int QualificationId { get; set; }
        public double Marks { get; set; }
    }
}

