using FysioApp.Data;
using FysioApp.Models;
using FysioApp.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FysioApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BusinessDbContext _business;

        public HomeController(ILogger<HomeController> logger, BusinessDbContext business)
        {
            _logger = logger;
            _business = business;
        }

        public async Task<IActionResult> Index()
        {            
            if(User.IsInRole(StaticDetails.StudentEndUser))
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var AppointmentList = 
                    await _business.Appointment
                    .Where(a => a.StudentId == userId)
                    .Where(a => a.DateTime.Date == DateTime.Today)
                    .Include(a => a.Patient)
                    .OrderBy(a => a.DateTime)
                    .ToListAsync();
                return View(AppointmentList);
            }
            return View();
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
