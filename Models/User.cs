using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.Models
{
    
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        public IEnumerable<Note>? Notes { get; set; }
        public IEnumerable<Reminder>? Reminders { get; set; }
    }
}