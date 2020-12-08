using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListWebApp.Context;
using ToDoListWebApp.Models;
using ToDoListWebApp.ViewModels.Notes;

namespace ToDoListWebApp.Controllers.NotesController
{
	[Authorize]
	public class IndexController : Controller
	{
		private readonly ToDoListContext _context;
		private string? _userid;
		
		public IndexController(ToDoListContext context)
		{
			_context = context;
		}
		
		public async Task<IActionResult> Index()
		{
			_userid = Request.Cookies["userid"];
			var notes = await _context.Notes.Where(note => note.IdUser.ToString() == _userid).ToListAsync();
			var reminders = await _context.Reminders.Where(reminder => reminder.IdUser.ToString() == _userid).ToListAsync();

			return View(new NoteViewModel() {Notes = notes, Reminders = reminders});
		}
		
		public async Task<IActionResult> CreateNote(NoteViewModel noteViewModel)
		{
			if (noteViewModel.Header == null)
				return RedirectToAction("Index", "Index");
			
			_userid = Request.Cookies["userid"];
			var note = new Note()
			{
				Header = noteViewModel.Header,
				Text = noteViewModel.Text,
				IdUser = new Guid(_userid),
				DateCreated = DateTime.Now,
				DateLastChanged = DateTime.Now
			};
			await _context.Notes.AddAsync(note);
			await _context.SaveChangesAsync();
			

			return RedirectToAction("Index", "Index");
		}

		public async Task<IActionResult> CreateReminder(NoteViewModel noteViewModel)
		{
			if (noteViewModel.Header == null)
				return RedirectToAction("Index", "Index");

			_userid = Request.Cookies["userid"];
			var reminder = new Reminder()
			{
				Header = noteViewModel.Header,
				Text = noteViewModel.Text,
				IdUser = new Guid(_userid),
				DateCreated = DateTime.Now,
				DateLastChanged = DateTime.Now,
				ReminderDate = noteViewModel.ReminderDate,
				StatusNote = noteViewModel.StatusNote
			};
			await _context.Reminders.AddAsync(reminder);
			await _context.SaveChangesAsync();


			return RedirectToAction("Index", "Index");
		}

		public async Task<IActionResult> DeleteNote(string idNote)
		{
			var note = await _context.Notes.FindAsync(new Guid(idNote));
			if (note == null)
			{
				return NotFound();
			}

			_context.Notes.Remove(note);
			await _context.SaveChangesAsync();
			
			return RedirectToAction("Index", "Index");
		}

		public async Task<IActionResult> DeleteReminder(string idReminder)
		{
			var reminder = await _context.Reminders.FindAsync(new Guid(idReminder));
			if (reminder == null)
			{
				return NotFound();
			}

			_context.Reminders.Remove(reminder);
			await _context.SaveChangesAsync();

			return RedirectToAction("Index", "Index");
		}
	}
}
