using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parky2Web.Models;
using Parky2Web.Repository.IRepository;
using Parky2Web.StaticData;

namespace Parky2Web.Controllers
{
    public class NationalParksController : Controller
    {

        private readonly INationalParkRepository _npRepo;

        public NationalParksController(INationalParkRepository npRepo)
        {
            _npRepo = npRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            var listNationalPark = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);

            return Json(new { data = listNationalPark });
        }

      
        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark nationalPark = new NationalPark();
            if (id == null)
            {

                return NotFound();
            }

            nationalPark = await _npRepo.GetByIdAsync(id.GetValueOrDefault(),SD.NationalParkAPIPath);

            return View(nationalPark);


        }
        [HttpPost]
        public async Task<IActionResult> Upsert(NationalPark national)
        {
            if (national == null)
            {
                return View(national);
            }

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fo = files[0].OpenReadStream())
                    {
                        using (var mo = new MemoryStream())
                        {
                            fo.CopyTo(mo);
                            p1 = mo.ToArray();
                        }
                    }
                    national.Picture = p1;
                }
                else
                {
                    var objectform = await _npRepo.GetByIdAsync(national.Id,SD.APIBaseUrl );
                    national.Picture = objectform.Picture;
                }

                if (national.Id == 0)
                {
                    if ( await _npRepo.CreateAsync(national,SD.NationalParkAPIPath))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(national);
                    }
                }
                else
                {
                    if (await _npRepo.UpdateAsync(national,SD.NationalParkAPIPath+national.Id))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(national);
                    }
                }
            }

            return View(national);

        }

        [HttpDelete]

        public async Task<IActionResult> Delete(int id)
        {
            if ( await _npRepo.DeleteAsync(id,SD.NationalParkAPIPath))
            {
                return Json(new { success = true, message = "its successed" });
            }
            else
            {
                return Json(new { success = true, message = "its successed" });
            }
        }
    }

}