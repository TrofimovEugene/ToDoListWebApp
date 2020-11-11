using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.Models
{
    public class Reminder
    {
	    [Key]
	    public Guid Id { get; set; }
	    public string Header { get; set; }
	    public string Text { get; set; }
	    public DateTime DateCreated { get; set; }
	    public DateTime DateLastChanged { get; set; }
	    public Guid IdUser { get; set; }
	    public StatusNote StatusNote { get; set; }
        public DateTime ReminderDate { get; set; }
        public User User { get; set; }
	}
}