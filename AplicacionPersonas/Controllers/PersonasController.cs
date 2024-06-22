using AplicacionPersonas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AplicacionPersonas.Controllers
{
    [Authorize]
    public class PersonasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Personas.ToListAsync());
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id, Nombres, Apellidos, NumeroIdentificacion, Email, TipoIdentificacion")] 
        Persona persona)
        {
            if (ModelState.IsValid)
            {
                if (EmailEsValido(persona.Email))
                {
                    persona.FechaCreacion = DateTime.Now;
                    _context.Add(persona);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Email", "El correo electrónico no es válido.");
                }
            }
            return View(persona);
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, 
            [Bind("Id, Nombres, Apellidos, NumeroIdentificacion, Email, TipoIdentificacion")] 
        Persona persona)
        {
            if (Id != persona.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (EmailEsValido(persona.Email))
                {
                    try
                    {
                        // Cargar la entidad desde la base de datos
                        var personaExistente = await _context.Personas.FindAsync(Id);
                        if (personaExistente == null)
                        {
                            return NotFound();
                        }

                        // Actualizar los campos necesarios
                        personaExistente.Nombres = persona.Nombres;
                        personaExistente.Apellidos = persona.Apellidos;
                        personaExistente.NumeroIdentificacion = persona.NumeroIdentificacion;
                        personaExistente.Email = persona.Email;
                        personaExistente.TipoIdentificacion = persona.TipoIdentificacion;

                        // Guardar los cambios
                        _context.Update(personaExistente);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PersonaExists(persona.Id))
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
                else
                {
                    ModelState.AddModelError("Email", "El correo electrónico no es válido.");
                }

            }
            return View(persona);
        }


        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas.FirstOrDefaultAsync(m => m.Id == Id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persona = await _context.Personas.FindAsync(id);
            _context.Personas.Remove(persona);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool PersonaExists(int Id)
        {
            return _context.Personas.Any(e => e.Id == Id);
        }

        // Método para verificar la validez del correo electrónico
        private bool EmailEsValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
