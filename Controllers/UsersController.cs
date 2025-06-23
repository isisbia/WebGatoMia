using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;
using WebGatoMia.Models;
using WebGatoMia.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using WebGatoMia.Data;

namespace WebGatoMia.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly GatoMiaDbContext _context;

        public UsersController(IUserService userService, IPasswordHasher<User> passwordHasher, GatoMiaDbContext context)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var user = await _userService.GetUserByIdAsync(id.Value);
            if (user == null) return NotFound();

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewBag.UserTypeId = new SelectList(_context.UserTypes, "Id", "Name");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserTypeId = new SelectList(_context.UserTypes, "Id", "Name", user.UserTypeId);
                return View(user);
            }

            // Cria novo usuário
            try
            {
                await _userService.CreateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao criar usuário: " + ex.Message);
                ViewBag.UserTypeId = new SelectList(_context.UserTypes, "Id", "Name", user.UserTypeId);
                return View(user);
            }
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var user = await _userService.GetUserByIdAsync(id.Value);
            if (user == null) return NotFound();

            ViewBag.UserTypeId = new SelectList(_context.UserTypes, "Id", "Name", user.UserTypeId);
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, User user)
        {
            if (id != user.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.UserTypeId = new SelectList(_context.UserTypes, "Id", "Name", user.UserTypeId);
                return View(user);
            }

            try
            {
                var existingUser = await _userService.GetUserByIdAsync(id);
                if (existingUser == null) return NotFound();

                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.UserTypeId = user.UserTypeId;
                existingUser.IsActive = user.IsActive;

                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, user.PasswordHash);
                }

                var updateResult = await _userService.UpdateUserAsync(existingUser);
                if (!updateResult)
                {
                    ModelState.AddModelError("", "Erro ao atualizar o usuário.");
                    ViewBag.UserTypeId = new SelectList(_context.UserTypes, "Id", "Name", user.UserTypeId);
                    return View(user);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro inesperado: " + ex.Message);
                ViewBag.UserTypeId = new SelectList(_context.UserTypes, "Id", "Name", user.UserTypeId);
                return View(user);
            }
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                {
                    ModelState.AddModelError("", "Erro ao deletar usuário.");
                    var user = await _userService.GetUserByIdAsync(id);
                    return View(user);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro inesperado: " + ex.Message);
                var user = await _userService.GetUserByIdAsync(id);
                return View(user);
            }
        }
    }
}
