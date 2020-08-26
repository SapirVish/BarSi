using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarSi.Data;
using BarSi.Models;
using System.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace BarSi.Controllers
{
    public class HospitalsController : Controller
    {
        private readonly BarSiContext _context;
        private static List<int> _recentlyOrdered = new List<int>();


        public HospitalsController(BarSiContext context)
        {
            _context = context;
            ViewData["IsAdmin"] = IsAdmin();
        }

        // GET: Hospitals
        public async Task<IActionResult> Index()
        {
            _recentlyOrdered.Clear();
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
                .Include(h => h.medicalEquipmentSupplies).ThenInclude(m => m.MedicalEquipment)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (hospital == null)
            {
                return NotFound();
            }

            ViewBag.Equipment = new SelectList(_context.MedicalEquipment, "Id", "Name");

            return View(hospital);
        }

        // GET: Hospitals/Create
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["Cities"] = new SelectList(_context.City, "Id", "Name");
            return View();
        }

        // POST: Hospitals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Hospital hospital, int City, double Latitude, double Longitude)
        {
            if (ModelState.IsValid)
            {
                hospital.City = _context.City.First(c => c.Id == City);
                Location hospitalLocation = new Location
                {
                    Lat = Latitude,
                    Lng = Longitude
                };
                hospital.Location = hospitalLocation;

                _context.Location.Add(hospitalLocation);
                _context.SaveChanges();
                _context.Add(hospital);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hospital);
        }
        
        [HttpPost]
        public async Task<IActionResult> Order(int HospitalId, int EquipmentId, int Quantity)
        {
            MedicalEquipmentSupply supply = _context.MedicalEquipmentSupply
                .FirstOrDefault(mes => mes.HospitalId == HospitalId && mes.MedicalEquipmentId == EquipmentId);

            if (supply != null)
            {
                supply.SupplyQuantity += Quantity;
                _context.MedicalEquipmentSupply.Update(supply);
            }
            else
            {
                supply = new MedicalEquipmentSupply(
                    _context.Hospital.First(h => h.Id == HospitalId),
                    _context.MedicalEquipment.First(e => e.Id == EquipmentId),
                    Quantity);

                _context.MedicalEquipmentSupply.Add(supply);
            }

            await _context.SaveChangesAsync();

            _recentlyOrdered.Add(EquipmentId);

            var ab = new { HospitalId = HospitalId };
            return Json(ab);
        }

        // @param id - the equipment-to-check-supply's id
        public JsonResult AvailableSupply(int? id)
        {
            var totalSupplyQuantity = _context.MedicalEquipment
                .Where(e => e.Id == id)
                .Select(e => e.Quantity);

            var result = _context.MedicalEquipmentSupply
                .Where(e => e.MedicalEquipmentId == id)
                .GroupBy(mes => mes.MedicalEquipmentId)
                .Select(g => new
                    {
                       // MedicalEquipmentId = g.Key,
                        TotalSupplied = g.Sum(mes => mes.SupplyQuantity)
                    });

            var available = totalSupplyQuantity.First();
           
            if (result.Count() != 0)
            {
                available -= result.FirstOrDefault().TotalSupplied;
            }

             return Json(available);
        }

        public JsonResult SuggestOrder(int? id)
        {
            // From all equipments, select equipment that wasn't recently ordered and that there's avaialable supply and we have the least of
            
            // Getting all equipments that weren't ordered recently in order of the quantity the given hospital currently have
            var relevantEquipment = _context.MedicalEquipment
                .Where(e => !_recentlyOrdered.Contains(e.Id))
                .Include(e => e.medicalEquipmentSupplies)
                .OrderBy(e => e.medicalEquipmentSupplies.Where(mes => mes.HospitalId == id).FirstOrDefault().SupplyQuantity)
                .ToList();

            MedicalEquipment suggestedOrder = null;
            int available = 0;

            // Getting an equipment with available supply
            foreach (var equipment in relevantEquipment)
            {
                available = (int) AvailableSupply(equipment.Id).Value;
              
                if (available > 0)
                {
                    suggestedOrder = equipment;

                    break;
                }
            };

            if (suggestedOrder == null || available == 0)
            {
               return null;
            }
             
            var suggestion = new
            { 
                EquipmentId = suggestedOrder.Id,
                EquipmentName = suggestedOrder.Name,
                CurrentQuantity = suggestedOrder.medicalEquipmentSupplies.Where(mes => mes.HospitalId == id).FirstOrDefault().SupplyQuantity,
                AvailableSupply = available
            };

            return Json(suggestion);
        }
    

        // GET: Hospitals/Edit/5
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
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

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

        private bool IsAdmin()
        {
            return (HttpContext != null) && (HttpContext.Session != null) &&
                                 (HttpContext.Session.GetString("IsAdmin") == "true");
        }
    }
}
