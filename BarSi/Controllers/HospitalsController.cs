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
    public class HospitalsController : Controller
    {
        private readonly BarSiContext _context;

        public HospitalsController(BarSiContext context)
        {
            _context = context;
        }

        // GET: Hospitals
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hospital.Include(h => h.City).Include(h => h.Location).ToListAsync());
        }

        // GET: Hospitals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospital
                .Include(h => h.City)
                .Include(h => h.Doctors).ThenInclude(d => d.City)
                .Include(h => h.Patients).ThenInclude(p => p.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // GET: Hospitals/Create
        public IActionResult Create()
        {
            ViewData["Cities"] = new SelectList(_context.City, "Id", "Name");
            return View();
        }

        // POST: Hospitals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Hospital hospital, int City)
        {
            if (ModelState.IsValid)
            {
                hospital.City = _context.City.First(c => c.Id == City);
                _context.Add(hospital);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hospital);
        }

        // GET: Hospitals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospital.Include(c => c.City).FirstAsync(h => h.Id == id);
            if (hospital == null)
            {
                return NotFound();
            }
            ViewData["Cities"] = new SelectList(_context.City, "Id", "Name", hospital.City.Id);
            return View(hospital);
        }

        // POST: Hospitals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Hospital hospital, int City)
        {
            if (id != hospital.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                hospital.City = _context.City.First(c => c.Id == City);

                try
                {
                    _context.Update(hospital);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospitalExists(hospital.Id))
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
            return View(hospital);
        }

        // GET: Hospitals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospital.Include(c => c.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // POST: Hospitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hospital = await _context.Hospital.FindAsync(id);
            _context.Hospital.Remove(hospital);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Hospitals/PatientsByHospitalCount
        public IActionResult PatientsByHospitalCount()
        {
            var patientsByHospital = from p in _context.Patient
                                     group p by p.Hospital.Id into g
                                     select new
                                     {
                                         g.Key,
                                         Count = g.Count()
                                     };

            var patientsCountByHospital = from ph in patientsByHospital
                                          join h in _context.Hospital
                                          on ph.Key equals h.Id
                                          select new { HospitalName = h.Name, ph.Count };

            return Json(patientsCountByHospital.ToList());
        }

        private bool HospitalExists(int id)
        {
            return _context.Hospital.Any(e => e.Id == id);
        }
    }
}
