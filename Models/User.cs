using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}