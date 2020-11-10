using Microsoft.EntityFrameworkCore;
using ToDoListWebApp.Models;

namespace ToDoListWebApp.Context
{
	public sealed class ToDoListContext: DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Note> Notes { get; set; }
		public ToDoListContext(DbContextOptions<ToDoListContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}
		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasMany(user => user.Notes)
				.WithOne(note => note.User)
				.IsRequired()
				.HasForeignKey(note => note.IdUser)
				.OnDelete(DeleteBehavior.Cascade);
		}
    }
}
