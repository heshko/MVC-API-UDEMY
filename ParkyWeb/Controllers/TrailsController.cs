using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository;
using ParkyWeb.StaticData;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {

        private readonly INationaParkRepository _npRepo;
        private readonly ITrailRepository _tRepo;

        public TrailsController(INationaParkRepository npRepo, ITrailRepository tRepo)
        {
            _npRepo = npRepo;
            _tRepo = tRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            var listNationalPark = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);

            VmTrail vmTrail = new VmTrail
            {
                NationalParkList = listNationalPark.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            if (id == null)
            {
            
                return View(vmTrail);
            }

            vmTrail.Trail = await _tRepo.GetAsync(SD.NationalParkAPIPath, id.GetValueOrDefault());

            return View(vmTrail);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert([Bind("Id","Name","State","Picture","Established")] VmTrail vmTrail)
        {
            var listNationalPark = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);

            VmTrail vmTrail2 = new VmTrail
            {
                NationalParkList = listNationalPark.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            if (!ModelState.IsValid)
            {
              
                return View(vmTrail2);
            }
           
            if (vmTrail.Trail.Id == 0)
            {
                if (await _tRepo.CreateAsync(SD.TrailAPIPath, vmTrail.Trail))
                {
                    return RedirectToAction("Index");
                }
                vmTrail2.Trail = vmTrail.Trail; 
                return View(vmTrail2);
            }

            if (await _tRepo.UpdateAsync(SD.TrailAPIPath + vmTrail.Trail.Id, vmTrail.Trail))
            {
                return RedirectToAction("Index");
            }

            vmTrail2.Trail = vmTrail.Trail;
            return View(vmTrail2);
        }

        public async Task<IActionResult> GetAllTrails()
        {
           
            var json =Json(new { data = await _tRepo.GetAllAsync(SD.TrailAPIPath) });
            //var data = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);
            return json;
        }

        [HttpDelete]
        public  async Task<IActionResult> Delete(int id)
        {
            if (await _tRepo.DeleteAsync(SD.TrailAPIPath,id))
            {
                return Json(new { success =true, message = "Delete Successful"});
            }
            else
            {
                return Json(new { success = false, message = "Delete not Successful" });
            }
        }


    }
}