using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ToDoListWebApp.Context;
using ToDoListWebApp.Models;
using ToDoListWebApp.ViewModels.Notes;

namespace ToDoListWebApp.Controllers.NotesController
{
    public class EditNoteController : Controller
    {
        private readonly ToDoListContext _context;

        public EditNoteController(ToDoListContext context)
        {
            _context = context;
        }
        // GET
        public async Task<IActionResult> EditNote(string idNote)
        {
            var note = await _context.Notes.FindAsync(new Guid(idNote));
            var model = new EditNoteViewModel()
            {
                Id = note.Id,
                Header = note.Header,
                Text = note.Text,
                DateCreated = note.DateCreated,
                DateLastChanged = note.DateLastChanged,
                IdUser = note.IdUser
            };
            return View(model);
        }
        [Route("EditNote/SaveChangesAsync")]
        public async Task<IActionResult> SaveChangesAsync(EditNoteViewModel editNoteViewModel)
        {
	        var note = await _context.Notes.FindAsync(editNoteViewModel.Id);
	        note.Header = editNoteViewModel.Header;
	        note.Text = editNoteViewModel.Text;
            note.DateLastChanged = DateTime.Now;

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
                if (!NoteExists(editNoteViewModel.Id))
                {
                    return RedirectToAction("Index", "Index");
                }

                throw;
            }
            
            return RedirectToAction("Index", "Index");
        }
        
        private bool NoteExists(Guid id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}