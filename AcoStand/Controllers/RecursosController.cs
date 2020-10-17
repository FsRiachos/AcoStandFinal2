using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AcoStand.Data;
using AcoStand.Models;

namespace AcoStand.Controllers
{
    public class RecursosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecursosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Recursos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Recursos.Include(r => r.Artigo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Recursos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recursos = await _context.Recursos
                .Include(r => r.Artigo)
                .FirstOrDefaultAsync(m => m.IdRecursos == id);
            if (recursos == null)
            {
                return NotFound();
            }

            return View(recursos);
        }

        // GET: Recursos/Create
        public IActionResult Create()
        {
            ViewData["ArtigoFK"] = new SelectList(_context.Artigos, "IdArtigo", "Contacto");
            return View();
        }

        // POST: Recursos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRecursos,Designacao,Tipo,ArtigoFK")] Recursos recursos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recursos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtigoFK"] = new SelectList(_context.Artigos, "IdArtigo", "Contacto", recursos.ArtigoFK);
            return View(recursos);
        }

        // GET: Recursos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recursos = await _context.Recursos.FindAsync(id);
            if (recursos == null)
            {
                return NotFound();
            }
            ViewData["ArtigoFK"] = new SelectList(_context.Artigos, "IdArtigo", "Contacto", recursos.ArtigoFK);
            return View(recursos);
        }

        // POST: Recursos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRecursos,Designacao,Tipo,ArtigoFK")] Recursos recursos)
        {
            if (id != recursos.IdRecursos)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recursos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecursosExists(recursos.IdRecursos))
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
            ViewData["ArtigoFK"] = new SelectList(_context.Artigos, "IdArtigo", "Contacto", recursos.ArtigoFK);
            return View(recursos);
        }

        // GET: Recursos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recursos = await _context.Recursos
                .Include(r => r.Artigo)
                .FirstOrDefaultAsync(m => m.IdRecursos == id);
            if (recursos == null)
            {
                return NotFound();
            }

            return View(recursos);
        }

        // POST: Recursos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recursos = await _context.Recursos.FindAsync(id);
            _context.Recursos.Remove(recursos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecursosExists(int id)
        {
            return _context.Recursos.Any(e => e.IdRecursos == id);
        }
    }
}
