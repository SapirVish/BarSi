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
    public class MedicalEquipmentsController : Controller
    {
        private readonly BarSiContext _context;

        public MedicalEquipmentsController(BarSiContext context)
        {
            _context = context;
            ViewData["IsAdmin"] = IsAdmin();
        }

        // GET: MedicalEquipments
        public async Task<IActionResult> Index()
        {
            return View(await _context.MedicalEquipment.ToListAsync());
        }

        // GET: MedicalEquipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalEquipment = await _context.MedicalEquipment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalEquipment == null)
            {
                return NotFound();
            }

            return View(medicalEquipment);
        }

        // GET: MedicalEquipments/Create
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: MedicalEquipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price")] MedicalEquipment medicalEquipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicalEquipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicalEquipment);
        }

        // GET: MedicalEquipments/Edit/5
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

            var medicalEquipment = await _context.MedicalEquipment.FindAsync(id);
            if (medicalEquipment == null)
            {
                return NotFound();
            }
            return View(medicalEquipment);
        }

        // POST: MedicalEquipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price")] MedicalEquipment medicalEquipment)
        {
            if (id != medicalEquipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalEquipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalEquipmentExists(medicalEquipment.Id))
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
            return View(medicalEquipment);
        }

        // GET: MedicalEquipments/Delete/5
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

            var medicalEquipment = await _context.MedicalEquipment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalEquipment == null)
            {
                return NotFound();
            }

            return View(medicalEquipment);
        }

        // POST: MedicalEquipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalEquipment = await _context.MedicalEquipment.FindAsync(id);
            _context.MedicalEquipment.Remove(medicalEquipment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalEquipmentExists(int id)
        {
            return _context.MedicalEquipment.Any(e => e.Id == id);
        }

        private bool IsAdmin()
        {
            return (HttpContext != null) && (HttpContext.Session != null) &&
                                 (HttpContext.Session.GetString("IsAdmin") == "true");
        }
    }
}
