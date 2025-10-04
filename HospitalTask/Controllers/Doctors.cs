using HospitalTask.Data;
using HospitalTask.Models;
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
       

        

        // إرجاع الأيام المتاحة من الداتا بيز
        public JsonResult GetAvailableDays(int doctorId)
        {
            var days = _Dbcontext.DoctorAvailabilities
                .Where(a => a.DoctorId == doctorId)
                .Select(a => a.DayOfWeek)
                .Distinct()
                .ToList();

            return Json(days);
        }

        // إرجاع الأوقات المتاحة حسب اليوم
        public JsonResult GetAvailableTimes(int doctorId, DateTime date)
        {
            var availability = _Dbcontext.DoctorAvailabilities
                .FirstOrDefault(a => a.DoctorId == doctorId && a.DayOfWeek == date.DayOfWeek);

            if (availability == null)
                return Json(new { times = new string[] { } });

            var bookedTimes = _Dbcontext.Appointments
                .Where(ap => ap.DoctorId == doctorId && ap.AppointmentDate.Date == date.Date)
                .Select(ap => ap.AppointmentDate.TimeOfDay)
                .ToList();

            var times = new System.Collections.Generic.List<string>();
            for (var t = availability.StartTime; t < availability.EndTime; t = t.Add(TimeSpan.FromMinutes(30)))
            {
                if (!bookedTimes.Contains(t))
                    times.Add(DateTime.Today.Add(t).ToString("HH:mm"));
            }

            return Json(new { times });
        }
        [HttpGet]
        public IActionResult CompleteAppointment(int doctorId)
        {
            ViewBag.DoctorId = doctorId;
            return View();
        }
        [HttpPost]
        public IActionResult CompleteAppointment(int doctorId,string patientname,DateTime myDate, string myTime)
        {
            //ViewBag.DoctorId = doctorId;
            var patientInfo = _Dbcontext.Patients.Where(p => p.FullName.Contains(patientname)).Select(p => p.PatientId).FirstOrDefault(); ;
            // var check_DoctorAvilability = _Dbcontext.DoctorAvailabilities.Where(a => a.appoin == appointmentdate);
            if (string.IsNullOrEmpty(patientname) || myDate == default || string.IsNullOrEmpty(myTime))
            {
                TempData["Error"] = "All fields are required.";
                return View( doctorId);

            }

            if (!TimeSpan.TryParse(myTime, out var selectedTime))
            {
                TempData["Error"] = "Invalid time format.";
                return View ( doctorId);
            }

            var appointmentDateTime = myDate.Date.Add(selectedTime);

            // check if time slot taken
            var isTaken = _Dbcontext.Appointments.Any(a =>
                a.DoctorId == doctorId &&
                a.AppointmentDate == appointmentDateTime
            );

            if (isTaken)
            {
                TempData["Error"] = "This time slot is already booked.";
                return View(doctorId);
            }

            // إضافة الحجز
            var appointment = new Appointment
            {
                DoctorId = doctorId,
                PatientId = patientInfo,

                AppointmentDate = appointmentDateTime,
               
            };

            _Dbcontext.Appointments.Add(appointment);
            _Dbcontext.SaveChanges();

            TempData["Success"] = "Appointment booked successfully!";
            return RedirectToAction("ConfirmAppointment");


        }

        // صفحة النجاح
        public IActionResult ConfirmAppointment()
        {
            return View();
        }
    

        public IActionResult ReservationsManagement()
        {
            var reservations = _Dbcontext.Appointments
       .Include(a => a.Doctor)   
       .Include(a => a.Patient)   
       .OrderBy(a => a.AppointmentDate)
       .ToList();

            return View(reservations);
        }
    }
}
