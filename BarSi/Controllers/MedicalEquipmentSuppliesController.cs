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
    public class MedicalEquipmentSuppliesController : Controller
    {
        private readonly BarSiContext _context;

        public MedicalEquipmentSuppliesController(BarSiContext context)
        {
            _context = context;
        }

        // GET: MedicalEquipmentSupplies
        public async Task<IActionResult> Index()
        {
            ViewData["IsAdmin"] = IsAdmin();
            var barSiContext = _context.MedicalEquipmentSupply.Include(m => m.Hospital).Include(m => m.MedicalEquipment);
            return View(await barSiContext.ToListAsync());
        }

        // GET: MedicalEquipmentSupplies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            IsAdmin();
            if (id == null)
            {
                return NotFound();
            }

            var medicalEquipmentSupply = await _context.MedicalEquipmentSupply
                .Include(m => m.Hospital)
                .Include(m => m.MedicalEquipment)
                .FirstOrDefaultAsync(m => m.HospitalId == id);
            if (medicalEquipmentSupply == null)
            {
                return NotFound();
            }

            return View(medicalEquipmentSupply);
        }

        // GET: MedicalEquipmentSupplies/Create
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["HospitalId"] = new SelectList(_context.Hospital, "Id", "Name");
            ViewData["MedicalEquipmentId"] = new SelectList(_context.MedicalEquipment, "Id", "Name");
            return View();
        }

        // POST: MedicalEquipmentSupplies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HospitalId,MedicalEquipmentId,SupplyQuantity")] MedicalEquipmentSupply medicalEquipmentSupply)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicalEquipmentSupply);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospital, "Id", "Id", medicalEquipmentSupply.HospitalId);
            ViewData["MedicalEquipmentId"] = new SelectList(_context.MedicalEquipment, "Id", "Name", medicalEquipmentSupply.MedicalEquipmentId);
            return View(medicalEquipmentSupply);
        }

        // GET: MedicalEquipmentSupplies/Edit/5
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

            var medicalEquipmentSupply = await _context.MedicalEquipmentSupply.FindAsync(id);
            if (medicalEquipmentSupply == null)
            {
                return NotFound();
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospital, "Id", "Id", medicalEquipmentSupply.HospitalId);
            ViewData["MedicalEquipmentId"] = new SelectList(_context.MedicalEquipment, "Id", "Name", medicalEquipmentSupply.MedicalEquipmentId);
            return View(medicalEquipmentSupply);
        }

        // POST: MedicalEquipmentSupplies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HospitalId,MedicalEquipmentId,SupplyQuantity")] MedicalEquipmentSupply medicalEquipmentSupply)
        {
            if (id != medicalEquipmentSupply.HospitalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalEquipmentSupply);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalEquipmentSupplyExists(medicalEquipmentSupply.HospitalId))
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
            ViewData["HospitalId"] = new SelectList(_context.Hospital, "Id", "Id", medicalEquipmentSupply.HospitalId);
            ViewData["MedicalEquipmentId"] = new SelectList(_context.MedicalEquipment, "Id", "Name", medicalEquipmentSupply.MedicalEquipmentId);
            return View(medicalEquipmentSupply);
        }

        // GET: MedicalEquipmentSupplies/Delete/5
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

            var medicalEquipmentSupply = await _context.MedicalEquipmentSupply
                .Include(m => m.Hospital)
                .Include(m => m.MedicalEquipment)
                .FirstOrDefaultAsync(m => m.HospitalId == id);
            if (medicalEquipmentSupply == null)
            {
                return NotFound();
            }

            return View(medicalEquipmentSupply);
        }

        // POST: MedicalEquipmentSupplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalEquipmentSupply = await _context.MedicalEquipmentSupply.FindAsync(id);
            _context.MedicalEquipmentSupply.Remove(medicalEquipmentSupply);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalEquipmentSupplyExists(int id)
        {
            return _context.MedicalEquipmentSupply.Any(e => e.HospitalId == id);
        }

        private bool IsAdmin()
        {
            bool isAdmin = (HttpContext != null) && (HttpContext.Session != null) &&
                                 (HttpContext.Session.GetString("IsAdmin") == "true");
            ViewData["IsAdmin"] = isAdmin;
            return isAdmin;
        }
    }
}
