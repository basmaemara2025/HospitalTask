namespace HospitalTask.Models
{
    public class Specialty
    {
        public int SpecialtyId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
