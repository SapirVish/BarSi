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

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
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
                patient.Hospital = _context.Hospital.First(h => h.Id == Hospital);
                patient.Doctor = _context.Doctor.First(d => d.Id == Doctor);
                patient.City = _context.City.First(c => c.Id == City);
                patient.Status = _context.PatientStatus.First(c => c.Id == Status);

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
        public async Task<IActionResult> Edit(int id, [Bind("MedicalBackgroundHispory,Id,FirstName,LastName,Birthdate")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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

            var patient = await _context.Patient
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
    }
}
