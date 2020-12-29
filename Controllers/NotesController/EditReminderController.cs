using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListWebApp.Context;
using ToDoListWebApp.ViewModels.Reminders;

namespace ToDoListWebApp.Controllers.NotesController
{
    public class EditReminderController : Controller
    {
        private readonly ToDoListContext _context;

        public EditReminderController(ToDoListContext context)
        {
            _context = context;
        }

        // GET
        public async Task<IActionResult> EditReminder(string idRem)
        {
            var note = await _context.Reminders.FindAsync(new Guid(idRem));
            var model = new ReminderViewModel()
            {
                Id = note.Id,
                Header = note.Header,
                Text = note.Text,
                DateCreated = note.DateCreated,
                DateLastChanged = note.DateLastChanged,
                ReminderDate = note.ReminderDate,
                StatusNote = note.StatusNote,
                IdUser = note.IdUser
            };
            return View(model);
        }

        [Route("EditNote/SaveChangesReminderAsync")]
        public async Task<IActionResult> SaveChangesReminderAsync(ReminderViewModel reminderViewModel)
        {
            var note = await _context.Reminders.FindAsync(reminderViewModel.Id);
            note.Header = reminderViewModel.Header;
            note.Text = reminderViewModel.Text;
            note.DateLastChanged = DateTime.Now;
            note.ReminderDate = new DateTime(reminderViewModel.ReminderDate.Ticks);
            note.StatusNote = reminderViewModel.StatusNote;

            try
            {
                _context.Entry(note).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReminderExists(reminderViewModel.Id))
                {
                    return RedirectToAction("Index", "Index");
                }

                throw;
            }
            
            return RedirectToAction("Index", "Index");
        }
        
        private bool ReminderExists(Guid id)
        {
            return _context.Reminders.Any(e => e.Id == id);
        }
    }
}