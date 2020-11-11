using System;

namespace ToDoListWebApp.ViewModels.Notes
{
    public class EditNoteViewModel
    {
        public Guid Id { get; set; }
        public string? Header { get; set; }
        public string? Text { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastChanged { get; set; } 
        public Guid IdUser { get; set; }
    }
}