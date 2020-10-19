using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository;
using ParkyWeb.StaticData;

namespace ParkyWeb.Controllers
{
    public class NationalParksController : Controller
    {

        private readonly INationaParkRepository _npRepo;

        public NationalParksController(INationaParkRepository npRepo)
        {
            _npRepo = npRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark nationalPark = new NationalPark();
            if (id == null)
            {
            
                return View(nationalPark);
            }

            nationalPark = await _npRepo.GetAsync(SD.NationalParkAPIPath, id.GetValueOrDefault());

            return View(nationalPark);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert([Bind("Id","Name","State","Picture","Established")] NationalPark nationalPark)
        {
            if (!ModelState.IsValid)
            {
                return View(nationalPark);
            }
            var files = HttpContext.Request.Form.Files;
            if (files.Count> 0)
            {
                byte[] p1 = null;
                using (var fs1 = files[0].OpenReadStream())
                {
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                }
                nationalPark.Picture = p1;
            }
            else
            {
                var objectform = await _npRepo.GetAsync(SD.APIBaseUrl, nationalPark.Id);
                nationalPark.Picture = objectform.Picture;
            }
           
            if (nationalPark.Id == 0)
            {
                if (await _npRepo.CreateAsync(SD.NationalParkAPIPath, nationalPark))
                {
                    return RedirectToAction("Index");
                } 
                return View(nationalPark);
            }

            if (await _npRepo.UpdateAsync(SD.NationalParkAPIPath + nationalPark.Id, nationalPark))
            {
                return RedirectToAction("Index");
            }


            return View(nationalPark);
        }

        public async Task<IActionResult> GetAllNationalParks()
        {
           
            var json =Json(new { data = await _npRepo.GetAllAsync(SD.NationalParkAPIPath) });
            //var data = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);
            return json;
        }

        [HttpDelete]
        public  async Task<IActionResult> Delete(int id)
        {
            if (await _npRepo.DeleteAsync(SD.NationalParkAPIPath,id))
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