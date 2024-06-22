using AplicacionPersonas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace AplicacionPersonas.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Nombre, Pass")] Usuario usuario)
        {
            // Verificar si el nombre de usuario ya existe en la base de datos
            if (_context.Usuarios.Any(u => u.Nombre == usuario.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre de usuario ya está en uso.");
            }

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(usuario.Pass))
                {
                    ModelState.AddModelError("Pass", "La contraseña es obligatoria.");
                    return View(usuario);
                }
                usuario.Pass = BCrypt.Net.BCrypt.HashPassword(usuario.Pass);
                usuario.FechaCreacion = DateTime.Now;
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(Id);
            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Pass = string.Empty;

            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Nombre,Pass")] Usuario usuario)
        {
            if (Id != usuario.Id)
            {
                return NotFound();
            }

            if (_context.Usuarios.Any(u => u.Nombre == usuario.Nombre && u.Id != Id))
            {
                ModelState.AddModelError("Nombre", "El nombre de usuario ya está en uso.");
            }

            if (ModelState.IsValid)
            {
                var existingUser = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == usuario.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                usuario.FechaCreacion = existingUser.FechaCreacion; // Preservar FechaCreacion

                // Hashear la nueva contraseña solo si se proporcionó una
                if (!string.IsNullOrEmpty(usuario.Pass))
                {
                    usuario.Pass = BCrypt.Net.BCrypt.HashPassword(usuario.Pass);
                }
                else
                {
                    usuario.Pass = existingUser.Pass;
                }

                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            return View(usuario);
        }



        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == Id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ususario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(ususario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool UsuarioExists(int Id)
        {
            return _context.Usuarios.Any(e => e.Id == Id);
        }
    }
}
