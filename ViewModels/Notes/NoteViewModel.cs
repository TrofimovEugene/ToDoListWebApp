#nullable enable
using System;
using System.Collections.Generic;
using ToDoListWebApp.Models;

namespace ToDoListWebApp.ViewModels.Notes
{
    public class NoteViewModel
    {
        public string? Header { get; set; }
        public string? Text { get; set; }
        public StatusNote StatusNote { get; set; }
        public DateTime ReminderDate { get; set; }
        // public DateTime DateCreated { get; set; }
        // public DateTime DateLastChanged { get; set; }
        // public Guid IdUser { get; set; }

        public List<Note> Notes { get; set; }
        public List<Reminder> Reminders { get; set; }
    }
}