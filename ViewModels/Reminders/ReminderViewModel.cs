using System;
using ToDoListWebApp.Models;

namespace ToDoListWebApp.ViewModels.Reminders
{
    public class ReminderViewModel
    {
        public Guid Id { get; set; }
        public string? Header { get; set; }
        public string? Text { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastChanged { get; set; } 
        public StatusNote StatusNote { get; set; }
        public DateTime ReminderDate { get; set; }
        public Guid IdUser { get; set; }
    }
}