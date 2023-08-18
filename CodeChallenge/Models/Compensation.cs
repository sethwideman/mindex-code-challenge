using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CodeChallenge.Models
{
    public class Compensation
    {

        [Key]
        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }

        public Employee Employee { get; set; }

        [Key]
        public decimal Salary { get; set; }

        [Key]
        public DateTime EffectiveDate { get; set; }
    }
}
