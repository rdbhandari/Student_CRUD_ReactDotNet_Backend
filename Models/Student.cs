using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? StudentName { get; set; }
        [Required]
        public string? StudentPhone { get; set; }
        [Required]
        public Address? StudentAddress { get; set; }
    }
    public class Address
    {
        public string? Street { get; set; }
        public int? Pincode { get; set; }
        public string? Country { get; set; }
    }
}
