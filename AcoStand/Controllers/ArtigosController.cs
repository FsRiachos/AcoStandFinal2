using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AcoStand.Data;
using AcoStand.Models;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Routing.Constraints;

namespace AcoStand.Controllers
{
    [Authorize(Roles = "User, Admin")]
    public class ArtigosController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        

        public ArtigosController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment environment)
        {
            _db = context;
            _userManager = userManager;
            _environment = environment;
        }


        // GET: Artigos
        public async Task<IActionResult> Index(int? pagina)
        {
            //Define o numero de itens por cada página
            const int itensPorPagina = 5;
            //se nao informado nº da página vai para a 1
            int numeroPagina = (pagina ?? 1);
            //obter a lsita dos artigos
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var utilizador = _db.Utilizadores.FirstOrDefault(a => a.UserFK == user.Id);

            var artigos = _db.Artigos.Select(a => new Artigos { 
                IdArtigo= a.IdArtigo,
                Categoria =a.Categoria,
                Contacto = a.Contacto,
                Descricao = a.Descricao,
                Dono = a.Dono,
                ListaFavUtilizador = a.ListaFavUtilizador.Where(b => b.IdUtlizador == utilizador.IdUtilizador).ToList(),
                Preco = a.Preco,
                Titulo = a.Titulo,
                FileName = a.FileName
            }).ToList();

            // var artigos = _db.Artigos.Include(a => a.Categoria).Include(a => a.Dono);

            return View(await artigos.ToPagedListAsync(numeroPagina, itensPorPagina));
        }



        // GET: Artigos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var artigos = await _db.Artigos
                .Include(a => a.Categoria)
                .Include(a => a.Dono)
                .FirstOrDefaultAsync(m => m.IdArtigo == id);
            if (artigos == null)
            {
                return NotFound();
            }
            return View(artigos);
        }
        // GET: Artigos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaFK"] = new SelectList(_db.Categorias, "IdCategoria", "Designacao");
            ViewData["DonoFK"] = new SelectList(_db.Utilizadores, "IdUtilizador", "Localidade");
            return View();
        }



        // POST: Artigos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdArtigo,Titulo,Preco,Descricao,Contacto,Validado,CategoriaFK")] Artigos artigos, IFormFile artigoImg)
        {
            if (ModelState.IsValid)
            {

                //Obter o user que está a criar o novo artigo e atribuir o seu id ao artigo que está a ser criado (IdDono)
                var user = await _userManager.GetUserAsync(HttpContext.User);
                artigos.Dono = _db.Utilizadores.Where(c => c.UserFK == user.Id).FirstOrDefault();

                //Colocar o artigo como não válido, será necessário o mesmo ser avaliado por um gestor antes de se tornar público
                artigos.Validado = false;

                if (artigoImg.Length > 0)
                {
                    var filePath = Path.Combine(_environment.WebRootPath, "ImagensArtigos");
                    var newFileName = Guid.NewGuid() + Path.GetExtension(artigoImg.FileName);

                    filePath = Path.Combine(filePath, newFileName);
                    artigos.FileName = newFileName;

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await artigoImg.CopyToAsync(stream);
                    }
                }

                _db.Add(artigos);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaFK"] = new SelectList(_db.Categorias, "IdCategoria", "Designacao", artigos.CategoriaFK);
            ViewData["DonoFK"] = new SelectList(_db.Utilizadores, "IdUtilizador", "Localidade", artigos.DonoFK);
            return View(artigos);
        }

        /*  public async Task<IActionResult> UploadImage(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();
                    /*var filePath = Path.Combine(_config["StoredFilesPath"],
            Path.GetRandomFileName());

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size });
        }*/

                    // GET: Artigos/Edit/5
                    public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var artigos = await _db.Artigos.FindAsync(id);
            if (artigos == null)
            {
                return NotFound();
            }
            ViewData["CategoriaFK"] = new SelectList(_db.Categorias, "IdCategoria", "Designacao", artigos.CategoriaFK);
            ViewData["DonoFK"] = new SelectList(_db.Utilizadores, "IdUtilizador", "Localidade", artigos.DonoFK);
            return View(artigos);
        }


        // POST: Artigos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdArtigo,Titulo,Preco,Descricao,Contacto,Validado,CategoriaFK")] Artigos artigos)
        {
            if (id != artigos.IdArtigo)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(artigos);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtigosExists(artigos.IdArtigo))
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
            ViewData["CategoriaFK"] = new SelectList(_db.Categorias, "IdCategoria", "Designacao", artigos.CategoriaFK);
            ViewData["DonoFK"] = new SelectList(_db.Utilizadores, "IdUtilizador", "Localidade", artigos.DonoFK);
            return View(artigos);
        }


        // GET: Artigos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var artigos = await _db.Artigos
                .Include(a => a.Categoria)
                .Include(a => a.Dono)
                .FirstOrDefaultAsync(m => m.IdArtigo == id);
            if (artigos == null)
            {
                return NotFound();
            }
            return View(artigos);
        }


        // POST: Artigos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artigos = await _db.Artigos.Include(a => a.ListaFavUtilizador).FirstOrDefaultAsync(a => a.IdArtigo == id);
            if (!String.IsNullOrWhiteSpace(artigos.FileName))
            {
                var filePath = Path.Combine(_environment.WebRootPath, "ImagensArtigos");
                filePath = Path.Combine(filePath, artigos.FileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            foreach (var artigo in artigos.ListaFavUtilizador)
            {
                _db.Favoritos.Remove(artigo);
            }
            
            _db.Artigos.Remove(artigos);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ArtigosExists(int id)
        {
            return _db.Artigos.Any(e => e.IdArtigo == id);
        }

          }
}