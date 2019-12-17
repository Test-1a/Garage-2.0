﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage_2._0.Models;

namespace Garage_2._0.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly Garage_2_0Context _context;

        public VehiclesController(Garage_2_0Context context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vehicle.ToListAsync());
        }

        // GET: Vehicles Overview
        public async Task<IActionResult> Overview()
        {
            var vehicles = await _context.Vehicle.ToListAsync();

            var model = vehicles.Select(v => new VehicleOverviewModel()
            {
                Typ = v.Typ,
                RegNr = v.RegNr,
                Color = v.Color,
                TimeOfParking = v.TimeOfParking
            }).ToList();

            return View(model);
        }

        // GET: Vehicles Other information
        public async Task<IActionResult> OtherDetails()
        {
            var vehicles = await _context.Vehicle.ToListAsync();

            var model = vehicles.Select(v => new VehicleOtherInfoModel()
            {
                NumnOfWheels = v.NumnOfWheels,
                Brand = v.Brand,
                Model = v.Model
            }).ToList();

            return View(model);
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RegNr,Typ,NumnOfWheels,Color,Model,Brand")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.TimeOfParking = DateTime.UtcNow;
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }



//=======
//>>>>>>> origin/Rod
        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        //public ActionResult EditAdmin(int id)
        //{
        //    var current = _context.Vehicle.Find(id);

        //}

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            // Adding the time of parking
            //var gettingVehicle = await _context.Vehicle.AsNoTracking().FindAsync(id);
            //var gettingVehicle = await _context.Vehicle.AsNoTracking().FindAsync(id);
            //var time = gettingVehicle.TimeOfParking;
            // https://stackoverflow.com/questions/26546891/how-keep-original-value-for-some-field-when-execute-edit-on-mvc

            var current = _context.Vehicle.FirstOrDefault(p => p.Id == id);

            //var current = _context.Vehicle.Find(id).AsNoTracking();

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Entry()
                    //vehicle.TimeOfParking = time;
                    //_context.Update(vehicle);
                    //await _context.SaveChangesAsync();
                    current.RegNr = vehicle.RegNr;
                    current.Typ = vehicle.Typ;
                    //current.NumnOfWheels = vehicle.NumnOfWheels;
                    current.Color = vehicle.Color;
                    current.Model = vehicle.Model;
                    current.Brand = vehicle.Brand;
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
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
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }

//<<<<<<< HEAD

        public IActionResult GetStatistics()
        {
            return View();
        }

        public async Task<IActionResult> GetGroupType()
        {

            // var model = await _context.Vehicle.ToListAsync();

            var model = await _context.Vehicle.GroupBy(v => v.Typ)
                                           .Select(group => new StatsViewModel
                                           {
                                               Count = group.Count(),
                                               VTyp = group.Key

                                           }).ToListAsync();
            return PartialView(nameof(GetGroupType), model);
        }

       
            public IActionResult CountOfWheels()
        {

            int SumOfWheels = (from s in  _context.Vehicle select s.NumnOfWheels).Sum();

            //_context.Vehicle.Sum(s => s.NumnOfWheels);

            
            var model = new StatsViewModel
            {
                SumOFwheels = SumOfWheels
            };

            return View (nameof(CountOfWheels),model);
        }


        public IActionResult TotalMinPrice()
        {

            var vehicle = _context.Vehicle.ToList();

            var endtime = DateTime.UtcNow;

            double totalmin = 0.0;

            foreach (var item in vehicle)
            {
                 totalmin = +(endtime - item.TimeOfParking).TotalMinutes;
            }



            var model = new StatsViewModel
            {
                TotalMin=totalmin,
                TotalPrice=(totalmin/60)*100

            };

            return PartialView(nameof(TotalMinPrice), model);
        }

//=======
        // Filter
        public async Task<IActionResult> Filter(string regnr)
        {
            var filtermodel = string.IsNullOrWhiteSpace(regnr) ?
                 await _context.Vehicle.ToListAsync() :
                await _context.Vehicle.Where(m => m.RegNr == regnr).ToListAsync();
            return View(nameof(Index), filtermodel);
        }

        //<<<<<<< HEAD***************************************************************************************

        // Receipt
        public async Task<IActionResult> Receipt(int? id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);

            var endtime = DateTime.UtcNow;
            var startime = vehicle.TimeOfParking;
            var parkingduration = endtime - startime;
            double parkingduration2 = (endtime - startime).TotalHours;
            double price = 100 * parkingduration2;

            var model = new ReceiptViewModel
            {
                RegNr = vehicle.RegNr,
                TimeOfParking = vehicle.TimeOfParking,
                TimeOfUnParking = endtime,
                TotalTimeOfParking = parkingduration2,
                Price = price
            };

            //var model = vehicles.Select(v => new VehicleOtherInfoModel()
            //{
            //    NumnOfWheels = v.NumnOfWheels,
            //    Brand = v.Brand,
            //    Model = v.Model
            //}).ToList();

            return View(model);
        }
//>>>>>>> origin/Rod
    }
}
