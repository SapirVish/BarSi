using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarSi.Data;
using BarSi.Models;
using Microsoft.AspNetCore.Http;

namespace BarSi.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly BarSiContext _context;

        public DoctorsController(BarSiContext context)
        {
            _context = context; 
            ViewData["IsAdmin"] = IsAdmin();
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Doctor.Include(d => d.City).Include(d => d.Hospital).ToListAsync());
        }

        public async Task<IActionResult> Search(string first_name, string last_name, DateTime birthdate, int city, int hospital)
        {
            var doctors = _context.Patient.AsQueryable();
            if (!String.IsNullOrEmpty(first_name))
                doctors = doctors.Where(d => d.FirstName.ToLower().Equals(first_name.ToLower()));
            if (!String.IsNullOrEmpty(last_name))
                doctors = doctors.Where(d => d.LastName.ToLower().Equals(last_name.ToLower()));
            if (birthdate != DateTime.MinValue)
                doctors = doctors.Where(d => d.Birthdate.Equals(birthdate));
            if (hospital != 0)
                doctors = doctors.Where(d => d.Hospital.Id == hospital);
            if (city != 0)
                doctors = doctors.Where(d => d.City.Id == city);


            var doctors_results = await doctors.Include(d => d.City).Include(d => d.Hospital).ToListAsync();
            var doctors_relevent_data = doctors_results.Select(d =>
            new
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Birthdate = d.Birthdate.ToString("dd-MM-yyyy"),
                Hospital = d.Hospital.Name,
                City = d.City.Name,
            }).ToList();

            return Json(doctors_relevent_data);
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor.Include(d => d.City).Include(d => d.Hospital)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["Hospitals"] = new SelectList(_context.Hospital, "Id", "Name");
            ViewData["Cities"] = new SelectList(_context.City, "Id", "Name");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Birthdate")] Doctor doctor, int Hospital, int City)
        {
            if (ModelState.IsValid)
            {
                UpdateComplexDoctorProps(doctor, Hospital, City);
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor.Include(d => d.City).Include(d => d.Hospital).FirstAsync(d => d.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }
            ViewData["Hospitals"] = new SelectList(_context.Hospital, "Id", "Name", doctor.Hospital.Id);
            ViewData["Cities"] = new SelectList(_context.City, "Id", "Name", doctor.City.Id);

            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Birthdate")] Doctor doctor, int Hospital, int City)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                UpdateComplexDoctorProps(doctor, Hospital, City);

                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor.Include(d => d.City).Include(d => d.Hospital)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctor.FindAsync(id);
            _context.Doctor.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Doctors/PatientsForDoctorStatistic
        public IActionResult PatientsForDoctorStatistic()
        {
            var patientsByDoctor = from p in _context.Patient
                                     group p by p.Doctor.Id into g
                                     select new
                                     {
                                         g.Key,
                                         Count = g.Count()
                                     };

            var patientsCountByDoctor = from pd in patientsByDoctor
                                          join d in _context.Doctor
                                          on pd.Key equals d.Id
                                          select new { DoctorId = d.Id, PatientsCount = pd.Count };

            var patientsForDoctorStatistic = from pd in patientsCountByDoctor
                                             group pd by pd.PatientsCount into g
                                             select new
                                             {
                                                 PatientsCount = g.Key,
                                                 DoctorsCount = g.Count()
                                             }; ;

            return Json(patientsForDoctorStatistic.ToList());
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctor.Any(e => e.Id == id);
        }

        private void UpdateComplexDoctorProps(Doctor doctor, int hospital, int city)
        {
            doctor.Hospital = _context.Hospital.First(h => h.Id == hospital);
            doctor.City = _context.City.First(c => c.Id == city);
        }

        private bool IsAdmin()
        {
            return (HttpContext != null) && (HttpContext.Session != null) &&
                                 (HttpContext.Session.GetString("IsAdmin") == "true");
        }
    }
}
