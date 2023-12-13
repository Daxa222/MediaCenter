using MediaCenter.Models;
using MediaCenter.Models.Data;
using MediaCenter.ViewModels.Chats;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MediaCenter.Controllers
{
    public class ChatsController : Controller
    {
        private readonly AppCtx _context;

        private readonly UserManager<User> _userManager;

        public ChatsController(AppCtx context,
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Chats
        public async Task<IActionResult> Index()
        {
            //через контекст данных получаем доступ к таблице базы данных Chats
            var appCtx = _context.Chats
                .Include(c => c.User)            //и связываем с таблицей пользователи через класс User
                .OrderBy(f => f.User);         //и сортируем все записи по пользователю


            //возвращаем в представление полученный список постов
            return View(await appCtx.ToListAsync());
        }

        // GET: Chats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Chats == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // GET: Chats/Create
        public async Task<IActionResult> Create()
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            ViewData["IdSender"] = new SelectList(_context.Users.OrderBy(o => o.Email), "Id", "Email", user.Id);
            ViewData["IdRecipient"] = new SelectList(_context.Users.OrderBy(o => o.Email), "Id", "Email", user.Id);
            return View();
        }

        // POST: Chats/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateChatViewModel model)
        {
            if (ModelState.IsValid)
            {
                Chat chat = new()
                {
                    DepartureDate = model.DepartureDate.Date,
                    Description = model.Description,
                    IdSender = model.IdSender,
                    IdRecipient = model.IdRecipient,
                };

                _context.Add(chat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdSender"] = new SelectList(_context.Users.OrderBy(o => o.Email), "Id", "Email", model.IdSender);
            ViewData["IdRecipient"] = new SelectList(_context.Users.OrderBy(o => o.Email), "Id", "Email", model.IdRecipient);
            return View(model);
        }

        // GET: Chats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Chats == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }

            EditChatViewModel model = new()
            {
                DepartureDate = chat.DepartureDate.Date,
                Description = chat.Description,
                IdSender = chat.IdSender,
                IdRecipient = chat.IdRecipient,
                
            };

            ViewData["IdSender"] = new SelectList(_context.Users.OrderBy(o => o.Email), "Id", "Email", model.IdSender);
            ViewData["IdRecipient"] = new SelectList(_context.Users.OrderBy(o => o.Email), "Id", "Email", model.IdRecipient);
            return View(model);
        }

        // POST: Chats/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditChatViewModel model)
        {
            Chat chat = await _context.Chats.FindAsync(id);

            if (id != chat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    chat.DepartureDate = model.DepartureDate.Date;
                    chat.Description = model.Description;
                    chat.IdSender = model.IdSender;
                    chat.IdRecipient = model.IdRecipient;

                    _context.Update(chat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatExists(chat.Id))
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
            ViewData["IdSender"] = new SelectList(_context.Users.OrderBy(o => o.Email), "Id", "Email", model.IdSender);
            ViewData["IdRecipient"] = new SelectList(_context.Users.OrderBy(o => o.Email), "Id", "Email", model.IdRecipient);
            return View(model);
        }

        // GET: Chats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Chats == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // POST: Chats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Chats == null)
            {
                return Problem("Entity set 'AppCtx.Chats'  is null.");
            }
            var chat = await _context.Chats.FindAsync(id);
            if (chat != null)
            {
                _context.Chats.Remove(chat);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatExists(int id)
        {
          return (_context.Chats?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
