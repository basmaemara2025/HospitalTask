namespace HospitalTask.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime DOB { get; set; }
        public string? Phone { get; set; }
        public string ?Email { get; set; }

        public ICollection<Appointment> Appointments { get; set; }=new List<Appointment>();
    }
}
