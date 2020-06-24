using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarSi.Data;
using BarSi.Models;

namespace BarSi.Controllers
{
    public class PatientsController : Controller
    {
        private readonly BarSiContext _context;

        public PatientsController(BarSiContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Patient.Include(p => p.City).Include(p => p.Doctor)
                .Include(p => p.Status).Include(p => p.Hospital).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Search(string name, DateTime birthdate, string hospital,
            string city, string status, string doctorName)
        {
            var patients = _context.Patient.AsQueryable();
            if (!String.IsNullOrEmpty(name))
                patients = patients.Where(p => p.FirstName.Contains(name) || p.LastName.Contains(name));
            if (birthdate != DateTime.MinValue)
                patients = patients.Where(p => p.Birthdate.Equals(birthdate));
            if (!String.IsNullOrEmpty(hospital))
                patients = patients.Where(p => p.Hospital.Name.Contains(hospital));
            if (!String.IsNullOrEmpty(city))
                patients = patients.Where(p => p.Hospital.Name.Contains(hospital));
            if (!String.IsNullOrEmpty(status))
                patients = patients.Where(p => p.Status.Status.Contains(status));
            if (!String.IsNullOrEmpty(doctorName))
                patients = patients.Where(p => p.Doctor.FirstName.Contains(doctorName) || p.Doctor.LastName.Contains(doctorName));

            var patients_results = await patients.Include(p => p.City).Include(p => p.Doctor)
                .Include(p => p.Status).Include(p => p.Hospital).ToListAsync();

            var patients_relevent_data = patients_results.Select(p =>
            new
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Birthdate = p.Birthdate.ToString("dd-MM-yyyy"),
                MedicalBackgroundHispory = p.MedicalBackgroundHispory,
                Hospital =p.Hospital.Name,
                City = p.City.Name,
                Status = p.Status.Status,
                DoctorName = p.Doctor.FirstName + ' ' + p.Doctor.LastName
            }).ToList();

            return Json(patients_relevent_data);
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .Include(p => p.Hospital).Include(p => p.Status).Include(p => p.City).Include(p => p.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            ViewData["Hospitals"] = new SelectList(_context.Hospital, "Id", "Name");
            ViewData["Doctors"] = from doctor in _context.Doctor
                                  select new SelectListItem { Text = doctor.FirstName + " " + doctor.LastName, Value = doctor.Id.ToString() };
            ViewData["Cities"] = new SelectList(_context.City, "Id", "Name");
            ViewData["PatientStatus"] = new SelectList(_context.PatientStatus, "Id", "Status");

            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicalBackgroundHispory,Id,FirstName,LastName,Birthdate")] Patient patient, int Hospital, int City, int Doctor, int Status)
        {

            if (ModelState.IsValid)
            {
                UpdateComplexPetientProps(patient, Hospital, City, Doctor, Status);

                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = (await _context.Patient.Include(p => p.City).Include(p => p.Doctor)
                .Include(p => p.Status).Include(p => p.Hospital).FirstAsync(p => p.Id == id));
            if (patient == null)
            {
                return NotFound();
            }

            ViewData["Hospitals"] = new SelectList(_context.Hospital, "Id", "Name", patient.Hospital.Id);
            ViewData["Doctors"] = from doctor in _context.Doctor
                                  select new SelectListItem { Text = doctor.FirstName + " " + doctor.LastName, Value = doctor.Id.ToString() };
            ViewData["Cities"] = new SelectList(_context.City, "Id", "Name", patient.City.Id);
            ViewData["PatientStatus"] = new SelectList(_context.PatientStatus, "Id", "Status", patient.Status.Id);

            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicalBackgroundHispory,Id,FirstName,LastName,Birthdate")] Patient patient, int Hospital, int City, int Doctor, int Status)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                UpdateComplexPetientProps(patient, Hospital, City, Doctor, Status);

                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient.Include(p => p.City).Include(p => p.Doctor)
                .Include(p => p.Status).Include(p => p.Hospital)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patient.FindAsync(id);
            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patient.Any(e => e.Id == id);
        }

        private void UpdateComplexPetientProps(Patient patient, int hospital, int city, int doctor, int status)
        {
            patient.Hospital = _context.Hospital.First(h => h.Id == hospital);
            patient.Doctor = _context.Doctor.First(d => d.Id == doctor);
            patient.City = _context.City.First(c => c.Id == city);
            patient.Status = _context.PatientStatus.First(c => c.Id == status);
        }
    }
}
