namespace HospitalTask.Models
{
    public class DoctorImage
    {
        public int DoctorImageId { get; set; }

        
        public string ImageName { get; set; } = null!;

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
    }
}
