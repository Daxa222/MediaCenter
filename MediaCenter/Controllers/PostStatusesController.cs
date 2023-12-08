using Humanizer.Localisation;
using MediaCenter.Models;
using MediaCenter.Models.Data;
using MediaCenter.ViewModels.PostStatuses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediaCenter.Controllers
{
    public class PostStatusesController : Controller
    {
        private readonly AppCtx _context;

        public PostStatusesController(AppCtx context)
        {
            _context = context;
        }

        // GET: PostStatuses
        public async Task<IActionResult> Index()
        {
            //через контекст данных получаем доступ к таблице базы данных PostStatuses
            //и сортируем все записи по имени статуса поста 
            var appCtx = _context.PostStatuses.OrderBy(f => f.StatusPost);


            //возвращаем в представление полученный список статусов
            return View(await appCtx.ToListAsync());
        }

        // GET: PostStatuses/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null || _context.PostStatuses == null)
            {
                return NotFound();
            }

            var postStatus = await _context.PostStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postStatus == null)
            {
                return NotFound();
            }

            return View(postStatus);
        }

        // GET: PostStatuses/Create
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostStatusViewModel model)
        {
            if (_context.PostStatuses
                .Where(f => f.StatusPost == model.StatusPost)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный статус уже существует");
            }

            if (ModelState.IsValid)
            {
                PostStatus postStatus = new()
                {
                    StatusPost = model.StatusPost
                };

                _context.Add(postStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: PostStatuses/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null || _context.PostStatuses == null)
            {
                return NotFound();
            }

            var postStatus = await _context.PostStatuses.FindAsync(id);
            if (postStatus == null)
            {
                return NotFound();
            }

            EditPostStatusViewModel model = new()
            {
                StatusPost = postStatus.StatusPost
            };
            return View(model);
        }


        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, EditPostStatusViewModel model)
        {
            PostStatus postStatus = await _context.PostStatuses.FindAsync(id);

            if (id != postStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    postStatus.StatusPost = model.StatusPost;
                    _context.Update(postStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostStatusExists(postStatus.Id))
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
            return View(model);
        }

        // GET: PostStatuses/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null || _context.PostStatuses == null)
            {
                return NotFound();
            }

            var postStatus = await _context.PostStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postStatus == null)
            {
                return NotFound();
            }

            return View(postStatus);
        }

        // POST: PostStatuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
            if (_context.PostStatuses == null)
            {
                return Problem("Entity set 'AppCtx.PostStatuses'  is null.");
            }
            var postStatus = await _context.PostStatuses.FindAsync(id);
            if (postStatus != null)
            {
                _context.PostStatuses.Remove(postStatus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostStatusExists(byte id)
        {
          return (_context.PostStatuses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
