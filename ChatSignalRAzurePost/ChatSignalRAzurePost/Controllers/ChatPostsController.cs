using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatSignalRAzurePost.Data;
using ChatSignalRAzurePost.Models;

namespace ChatSignalRAzurePost.Controllers
{
    public class ChatPostsController : Controller
    {
        private readonly ChatContext _context;

        public ChatPostsController(ChatContext context)
        {
            _context = context;
        }

        // GET: Chat
        public async Task<IActionResult> Index()
        {
            return _context.Posts != null ?
                        View(await _context.Posts.ToListAsync()) :
                        Problem("Entity set 'ChatContext.Posts'  is null.");
        }

        // GET: Chat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var chatPost = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatPost == null)
            {
                return NotFound();
            }

            return View(chatPost);
        }

        // GET: Chat/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string sender, string message)
        {
            ChatPost chatMessage = new()
            {
                Id = await this.GetMaxMessageAsync(),
                Message = message,
                Sender = sender
            };

            if (ModelState.IsValid)
            {
                await _context.Posts.AddAsync(chatMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chatMessage);
        }

        // GET: Chat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var chatPost = await _context.Posts.FindAsync(id);
            if (chatPost == null)
            {
                return NotFound();
            }
            return View(chatPost);
        }

        // POST: Chat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Message,Sender")] ChatPost chatPost)
        {
            if (id != chatPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatPostExists(chatPost.Id))
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
            return View(chatPost);
        }

        // GET: Chat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var chatPost = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatPost == null)
            {
                return NotFound();
            }

            return View(chatPost);
        }

        // POST: Chat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'ChatContext.Posts'  is null.");
            }
            var chatPost = await _context.Posts.FindAsync(id);
            if (chatPost != null)
            {
                _context.Posts.Remove(chatPost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<int> GetMaxMessageAsync()
        {
            int max = 0;

            if (await this._context.Posts.AnyAsync())
            {
                max = await this._context.Posts.MaxAsync(x => x.Id) + 1;
            }

            return max;
        }

        private bool ChatPostExists(int id)
        {
            return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
