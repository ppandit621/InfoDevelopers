using System.ComponentModel.DataAnnotations;

namespace InfoDevelopers.Models.Domain
{
    public class Employee
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        public string Gender { get; set; }
        public double? Salary { get; set; }
        [Required]
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        //public List<Qualification> Qualifications { get; set; }

        public ICollection<EmployeeQualification> EmployeeQualifications { get; set; }

    }
}
