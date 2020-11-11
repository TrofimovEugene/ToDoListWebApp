using System;

namespace ToDoListWebApp.Models
{
    public class Reminder : Note
    {
        public StatusNote StatusNote { get; set; }
        public DateTime ReminderDate { get; set; }
    }
}