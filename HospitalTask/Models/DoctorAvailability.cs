using Microsoft.EntityFrameworkCore;

namespace HospitalTask.Models
{
    public class DoctorAvailability
    { 
        public int DoctorAvailabilityId { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;

        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
