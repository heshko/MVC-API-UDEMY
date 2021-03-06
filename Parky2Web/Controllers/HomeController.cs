﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Parky2Web.Models;
using Parky2Web.Models.ViewModel;
using Parky2Web.Repository.IRepository;
using Parky2Web.StaticData;

namespace Parky2Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailsRepository _tRepo;

        public HomeController(ILogger<HomeController> logger,INationalParkRepository npRepo,ITrailsRepository tRepo)
        {
            _logger = logger;
            _npRepo = npRepo;
            _tRepo = tRepo;
        }

        public async Task<IActionResult> Index()
        {
            IndexVm all = new IndexVm
            {
                NationalParkList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath),
                TrailList = await _tRepo.GetAllAsync(SD.TrailAPIPath)
            };
            return View(all);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
