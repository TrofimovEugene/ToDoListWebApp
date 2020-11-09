using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToDoListWebApp.Context;
using ToDoListWebApp.Models;
using ToDoListWebApp.ViewModels;

namespace ToDoListWebApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ToDoListContext _context;
		private string _userid;
		public HomeController(ToDoListContext context)
		{
			_context = context;
		}
		
		[Authorize]
		public async Task<IActionResult> Index()
		{
			_userid = Request.Cookies["userid"];
			var notes = await _context.Notes.Where(note => note.IdUser.ToString() == _userid).ToListAsync();
			return View(new NoteViewModel() {Notes = new List<Note>(notes)});
		}
		
		[Authorize]
		public async Task<IActionResult> CreateNote(NoteViewModel noteViewModel)
		{
			if (noteViewModel.Header == null)
				return RedirectToAction("Index", "Home");
			
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
			

			return RedirectToAction("Index", "Home");
		}

		[Authorize]
		public async Task<IActionResult> DeleteNote(string idNote)
		{
			var note = await _context.Notes.FindAsync(new Guid(idNote));
			if (note == null)
			{
				return NotFound();
			}

			_context.Notes.Remove(note);
			await _context.SaveChangesAsync();
			
			return RedirectToAction("Index", "Home");
		}
	}
}
