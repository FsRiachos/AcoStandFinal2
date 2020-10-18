using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcoStand.Data;
using AcoStand.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcoStand.Controllers
{
    public class FavoritosController : Controller
    {
        private readonly ApplicationDbContext _db;

        private readonly UserManager<IdentityUser> _userManager;
        public FavoritosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _db = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var utilizador = _db.Utilizadores.FirstOrDefault(a => a.UserFK == user.Id);

            var favoritos = _db.Favoritos.Include(a => a.Artigo).Include(a => a.Artigo.Categoria).Where(a => a.IdUtlizador == utilizador.IdUtilizador);

            return View(favoritos);
        }

        [HttpPost]
       
        public async Task<IActionResult> UpdateFavorito(int id, bool status)
        {


            var artigo = _db.Artigos.FirstOrDefault(a => a.IdArtigo == id);

            if (artigo == null)
            {
                return NotFound();
            }

            //Obter o user que está a chamar a função
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var utilizador = _db.Utilizadores.FirstOrDefault(a => a.UserFK == user.Id);

            var favorito = _db.Favoritos.FirstOrDefault(a => (a.IdArtigo == id) && (a.IdUtlizador == utilizador.IdUtilizador));

            if (status)
            {
                if (favorito == null)
                {
                    _db.Add(new Favoritos { IdArtigo = id, IdUtlizador = utilizador.IdUtilizador });
                }
            }
            else
            {
                _db.Favoritos.Remove(favorito);
            }

            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
