using MediaCenter.Models;
using MediaCenter.Models.Data;
using MediaCenter.ViewModels.Posts;
using MediaCenter.ViewModels.PostStatuses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MediaCenter.Controllers
{
    public class PostsController : Controller
    {
        private readonly AppCtx _context;

        private readonly UserManager<User> _userManager;

        public PostsController(AppCtx context,
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            //через контекст данных получаем доступ к таблице базы данных PostStatuses
            var appCtx = _context.Posts
                .Include(p => p.PostStatus)      //и связываем с таблицей статусы постов через класс PostStatus
                .Include(p => p.User)           //и связываем с таблицей пользователи через класс User
                .OrderBy(f => f.Description);  //и сортируем все записи по описанию поста


            //возвращаем в представление полученный список постов
            return View(await appCtx.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.PostStatus)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public async Task<IActionResult> CreateAsync()
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            ViewData["IdStatus"] = new SelectList(_context.PostStatuses.OrderBy(o=>o.StatusPost), "Id", "StatusPost");
            ViewData["IdAuthor"] = new SelectList(_context.Users.OrderBy(o=>o.Email), "Id", "Email", user.Id);
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            if (_context.Posts
                .Where(f => f.StatusPostTitlePost == model.StatusPostTitlePost)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Данный пост уже существует");
            }

            if (ModelState.IsValid)
            {
                Post post = new()
                {
                    StatusPostTitlePost = model.StatusPostTitlePost,
                    Multimedia = model.Multimedia,
                    Description = model.Description,
                    IdStatus = model.IdStatus,
                    IdAuthor = model.IdAuthor
                };

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdStatus"] = new SelectList(_context.PostStatuses.OrderBy(o => o.StatusPost), "Id", "StatusPost", model.IdStatus);
            ViewData["IdAuthor"] = new SelectList(_context.Users.OrderBy(o => o.Email), "Id", "Email", model.IdAuthor);
            return View(model);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            EditPostViewModel model = new()
            {
                StatusPostTitlePost = post.StatusPostTitlePost,
                Multimedia = post.Multimedia,
                Description = post.Description,
                IdStatus = post.IdStatus,
                DatePublication = post.DatePublication.Date,
                IdAuthor = post.IdAuthor
            };

            ViewData["IdStatus"] = new SelectList(_context.PostStatuses.OrderBy(o => o.StatusPost), "Id", "StatusPost", model.IdStatus);
            ViewData["IdAuthor"] = new SelectList(_context.Users.OrderBy(o => o.Email), "Id", "Email", model.IdAuthor);

            return View(model);
        }


        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPostViewModel model)
        {
            Post post = await _context.Posts.FindAsync(id);

            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    post.StatusPostTitlePost = model.StatusPostTitlePost;
                    post.Multimedia = model.Multimedia;
                    post.Description = model.Description;
                    post.IdStatus = model.IdStatus;
                    post.DatePublication = model.DatePublication;
                    post.IdAuthor = model.IdAuthor;
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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
            ViewData["IdStatus"] = new SelectList(_context.PostStatuses.OrderBy(o => o.StatusPost), "Id", "StatusPost", model.IdStatus);
            ViewData["IdAuthor"] = new SelectList(_context.Users.OrderBy(o => o.Email), "Id", "Email", model.IdAuthor);
            return View(model);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.PostStatus)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'AppCtx.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
