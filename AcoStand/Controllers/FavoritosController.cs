using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcoStand.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcoStand.Controllers
{
    public class FavoritosController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FavoritosController(ApplicationDbContext context)
        {
            _db = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _db.Favoritos.ToListAsync()); ;
        }
    }
}
