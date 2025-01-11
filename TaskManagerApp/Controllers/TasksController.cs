using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Models;
using System.Threading.Tasks;

namespace TaskManagerApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskDbContext _context;

        public TasksController(TaskDbContext context)
        {
            _context = context;
        }

        // GET: Tasks/Analytics
        public async Task<IActionResult> Analytics()
        {
            // Summary statistics
            var totalTasks = await _context.UserTasks.CountAsync();
            var completedTasks = await _context.UserTasks.CountAsync(t => t.IsCompleted);
            var pendingTasks = totalTasks - completedTasks;
            var isCompletedLabels = "'Completed', 'Not Completed'";
            var isCompletedCounts = $"{completedTasks}, {pendingTasks}";

            // Pass data to the view
            var analyticsData = new
            {
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                PendingTasks = pendingTasks,
                IsCompletedLabels = isCompletedLabels,
                IsCompletedCounts = isCompletedCounts
            };

            return View(analyticsData);
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserTasks.ToListAsync());
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,DueDate,IsCompleted")] UserTask userTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userTask);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Task created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(userTask);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTask = await _context.UserTasks.FindAsync(id);
            if (userTask == null)
            {
                return NotFound();
            }
            return View(userTask);
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DueDate,IsCompleted")] UserTask userTask)
        {
            if (id != userTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userTask);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Task updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTaskExists(userTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "An error occurred while updating the task. Please try again.";
                        return View(userTask);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userTask);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTask = await _context.UserTasks.FirstOrDefaultAsync(m => m.Id == id);
            if (userTask == null)
            {
                return NotFound();
            }

            return View(userTask);
        }

        // POST: Task/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userTask = await _context.UserTasks.FindAsync(id);
            if (userTask != null)
            {
                _context.UserTasks.Remove(userTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTaskExists(int id)
        {
            return _context.UserTasks.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, bool isCompleted)
        {
            var task = await _context.UserTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.IsCompleted = isCompleted;
            await _context.SaveChangesAsync();

            return Ok(); // Respond with a success message
        }
    }
}
