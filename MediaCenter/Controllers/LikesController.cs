using MediaCenter.Models;
using MediaCenter.Models.Data;
using MediaCenter.ViewModels.Likes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MediaCenter.Controllers
{
    public class LikesController : Controller
    {
        private readonly AppCtx _context;

        private readonly UserManager<User> _userManager;

        public LikesController(AppCtx context,
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Likes
        public async Task<IActionResult> Index()
        {
            //через контекст данных получаем доступ к таблице базы данных Likes
            var appCtx = _context.Likes
                .Include(l => l.Post)             //и связываем с таблицей постов через класс Post
                .Include(l => l.User)            //и связываем с таблицей пользователи через класс User
                .OrderBy(f => f.IdUser);        //и сортируем все записи по пользователю


            //возвращаем в представление полученный список постов
            return View(await appCtx.ToListAsync());
        }

        // GET: Likes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Likes == null)
            {
                return NotFound();
            }

            var like = await _context.Likes
                .Include(l => l.Post)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // GET: Likes/Create
        public async Task<IActionResult> Create()
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            ViewData["IdPost"] = new SelectList(_context.Posts.OrderBy(o => o.StatusPostTitlePost), "Id", "StatusPostTitlePost");
            ViewData["IdUser"] = new SelectList(_context.Users.OrderBy(o => o.Email), "Id", "Email", user.Id);
            return View();
        }

        // POST: Likes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLikeViewModel model)
        {
            if (_context.Likes
                .Where(f => f.IdPost == model.IdPost && f.IdUser == model.IdUser)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Лайк у поста уже существует");
            }

            if (ModelState.IsValid)
            {
                Like like = new()
                {
                    IdPost = model.IdPost,
                    IdUser = model.IdUser,
                    InstallationDate = model.InstallationDate.Date
                };

                _context.Add(like);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdPost"] = new SelectList(_context.Posts.OrderBy(o => o.StatusPostTitlePost), "Id", "StatusPostTitlePost", model.IdPost);
            ViewData["IdUser"] = new SelectList(_context.Users.OrderBy(o => o.Email), "Id", "Email", model.IdUser);
            return View(model);
        }

        // GET: Likes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Likes == null)
            {
                return NotFound();
            }

            var like = await _context.Likes.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }

            EditLikeViewModel model = new()
            {
                IdPost = like.IdPost,
                IdUser = like.IdUser,
                InstallationDate = like.InstallationDate.Date
            };

            ViewData["IdPost"] = new SelectList(_context.Posts.OrderBy(o => o.StatusPostTitlePost), "Id", "StatusPostTitlePost", model.IdPost);
            ViewData["IdUser"] = new SelectList(_context.Users.OrderBy(o => o.Email), "Id", "Email", model.IdUser);
            return View(model);
        }

        // POST: Likes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditLikeViewModel model)
        {
            Like like = await _context.Likes.FindAsync(id);

            if (id != like.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    like.IdPost = model.IdPost;
                    like.IdUser = model.IdUser;
                    like.InstallationDate = model.InstallationDate.Date;
                    _context.Update(like);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LikeExists(like.Id))
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
            ViewData["IdPost"] = new SelectList(_context.Posts.OrderBy(o => o.StatusPostTitlePost), "Id", "StatusPostTitlePost", model.IdPost);
            ViewData["IdUser"] = new SelectList(_context.Users.OrderBy(o => o.Email), "Id", "Email", model.IdUser);
            return View(model);
        }

        // GET: Likes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Likes == null)
            {
                return NotFound();
            }

            var like = await _context.Likes
                .Include(l => l.Post)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // POST: Likes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Likes == null)
            {
                return Problem("Entity set 'AppCtx.Likes'  is null.");
            }
            var like = await _context.Likes.FindAsync(id);
            if (like != null)
            {
                _context.Likes.Remove(like);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LikeExists(int id)
        {
          return (_context.Likes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
