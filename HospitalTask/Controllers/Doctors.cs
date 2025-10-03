using HospitalTask.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalTask.Controllers
{
    public class Doctors : Controller
    {
        private ApplicationDbContext _Dbcontext = new();
        public IActionResult Index(int? specialty,string DoctorName,int page=1)
        {
            var doctors = _Dbcontext.Doctors.Include(s => s.Specialty).AsQueryable();
            var totalpages = Math.Ceiling(doctors.Count() / 3.0);
            ViewBag.Totalpages = totalpages;
            doctors = doctors.Skip((page - 1) * 3).Take(3);

            var Specialities = _Dbcontext.Specialties;
            ViewBag.Specialties = Specialities.AsEnumerable();
            if (specialty is not null) 
            {
                doctors = _Dbcontext.Doctors.Where(d => d.SpecialtyId == specialty);
            }
            if (DoctorName is not null)
            {
                doctors = _Dbcontext.Doctors.Where(d => d.FullName.Contains(DoctorName));
            }
            ViewBag.DoctorName = DoctorName;
            ViewBag.Dspecialty = specialty;
            ViewBag.currentpage = page;

            return View("BookAppointment",doctors.ToList());
        }
        public IActionResult CompleteAppointment(int doctorId,string patientname,DateTime appointmentdate,string time )
        {
            var patientInfo = _Dbcontext.Patients.Where(p => p.FullName.Contains(patientname));
           // var check_DoctorAvilability=_Dbcontext.DoctorAvailabilities.Where(a=>a.DayOfWeek== appointmentdate)
            return View();
        }
        public IActionResult ReservationsManagement()
        {
            return View();
        }
    }
}
