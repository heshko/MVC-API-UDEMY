using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Parky2Web.Models;
using Parky2Web.Models.ViewModel;
using Parky2Web.Repository.IRepository;
using Parky2Web.StaticData;

namespace Parky2Web.Controllers
{
    public class TrailsController : Controller
    {

        private readonly INationalParkRepository _npRepo;
        private readonly ITrailsRepository _tRepo;

        public TrailsController(INationalParkRepository npRepo, ITrailsRepository tRepo)
        {
            _npRepo = npRepo;
            _tRepo = tRepo;
        }
        public IActionResult Index()
        {
            return View( new Trail());
        }

        public async Task<IActionResult> GetAllTrails()
        {
            var listTrails = await _tRepo.GetAllAsync(SD.TrailAPIPath);

            return Json(new { data = listTrails });
        }

     
        public async Task<IActionResult> Upsert(int? id)
        {
            var nationalParksList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);

            TrailsVm trailsVm = new TrailsVm
            {
                ListNationalParks = nationalParksList.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            if (id == null)
            {

                return View(trailsVm);
            }

            trailsVm.Trail = await _tRepo.GetByIdAsync(id.GetValueOrDefault(),SD.TrailAPIPath);

            return View(trailsVm);


        }
        [HttpPost]
        public async Task<IActionResult> Upsert(TrailsVm trailsVm)
        {
            if (trailsVm == null)
            {
                return View(trailsVm);
            }

            if (ModelState.IsValid)
            {
               

                if (trailsVm.Trail.Id == 0)
                {
                    if ( await _tRepo.CreateAsync(trailsVm.Trail,SD.TrailAPIPath))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(trailsVm);
                    }
                }
                else
                {
                    if (await _tRepo.UpdateAsync(trailsVm.Trail,SD.TrailAPIPath+trailsVm.Trail.Id))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(trailsVm);
                    }
                }
            }
            else
            {
                var nationalParksList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);
                TrailsVm trailsVm2 = new TrailsVm
                {
                    ListNationalParks = nationalParksList.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    })
                };
                return View(trailsVm2);

            }
           

          

        }

        [HttpDelete]

        public async Task<IActionResult> Delete(int id)
        {
            if ( await _tRepo.DeleteAsync(id,SD.TrailAPIPath))
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