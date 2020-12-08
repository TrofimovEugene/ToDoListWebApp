using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.Models
{
    
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        [EmailAddress]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        
        public IEnumerable<Note>? Notes { get; set; }
        public IEnumerable<Reminder>? Reminders { get; set; }
    }
}