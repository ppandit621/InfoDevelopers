using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InfoDevelopers.Models.Domain
{
    public class EmployeeQualification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double Marks { get; set; }
        [Required]
        public Guid EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }
        [Required]
        public int QualificationId { get; set; }
        [ForeignKey("QualificationId")]
        public Qualification? Qualification { get; set; }
    }
}
