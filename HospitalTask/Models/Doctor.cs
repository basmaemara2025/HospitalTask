namespace HospitalTask.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; } = null!;
        public string Phone { get; set; }= null!;
        public string? Email { get; set; }

        public int SpecialtyId { get; set; }
        public Specialty Specialty { get; set; } = null!;
        public string MainImageName { get; set; } = null!;

        public ICollection<DoctorAvailability> Availabilities { get; set; }=new List<DoctorAvailability>();
        public ICollection<Appointment> Appointments { get; set; }=new List<Appointment>();
    }
}
