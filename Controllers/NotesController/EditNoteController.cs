using Microsoft.AspNetCore.Mvc;

namespace ToDoListWebApp.Controllers.NotesController
{
    public class EditNoteController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}