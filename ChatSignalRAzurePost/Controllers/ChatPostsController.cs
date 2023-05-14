using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // GET: ChatPosts
        public async Task<IActionResult> Index()
        {
            return _context.ChatPosts != null ?
                        View(await _context.ChatPosts.OrderByDescending(x => x.Id).ToListAsync()) :
                        Problem("Entity set 'ChatContext.ChatPosts'  is null.");
        }

        // GET: ChatPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ChatPosts == null)
            {
                return NotFound();
            }

            var chatPost = await _context.ChatPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatPost == null)
            {
                return NotFound();
            }

            return View(chatPost);
        }

        private async Task<int> GetMaxChatPostAsync()
        {
            int maxId = 1;

            if (await _context.ChatPosts.AnyAsync())
            {
                maxId = await this._context.ChatPosts.MaxAsync(x => x.Id) + 1;
            }

            return maxId;
        }

        // POST: ChatPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string message, string sender)
        {
            if (ModelState.IsValid)
            {
                ChatPost chatPost = new()
                {
                    Id = await GetMaxChatPostAsync(),
                    Message = message,
                    Sender = sender
                };

                await _context.ChatPosts.AddAsync(chatPost);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ChatPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ChatPosts == null)
            {
                return NotFound();
            }

            var chatPost = await _context.ChatPosts.FindAsync(id);
            if (chatPost == null)
            {
                return NotFound();
            }
            return View(chatPost);
        }

        // POST: ChatPosts/Edit/5
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

        // GET: ChatPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ChatPosts == null)
            {
                return NotFound();
            }

            var chatPost = await _context.ChatPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatPost == null)
            {
                return NotFound();
            }

            return View(chatPost);
        }

        // POST: ChatPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ChatPosts == null)
            {
                return Problem("Entity set 'ChatContext.ChatPosts'  is null.");
            }
            var chatPost = await _context.ChatPosts.FindAsync(id);
            if (chatPost != null)
            {
                _context.ChatPosts.Remove(chatPost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatPostExists(int id)
        {
            return (_context.ChatPosts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
