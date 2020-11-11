using Microsoft.EntityFrameworkCore;
using ToDoListWebApp.Models;

namespace ToDoListWebApp.Context
{
	public sealed class ToDoListContext: DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Note> Notes { get; set; }
		public DbSet<Reminder> Reminders { get; set; }
		public ToDoListContext(DbContextOptions<ToDoListContext> options)
			: base(options)
		{
		}
		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Note>()
				.HasOne(note => note.User)
				.WithMany( user => user.Notes)
				.IsRequired()
				.HasForeignKey(note => note.IdUser)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Reminder>()
				.HasOne(reminder => reminder.User)
				.WithMany(user => user.Reminders)
				.IsRequired()
				.HasForeignKey(reminder => reminder.IdUser)
				.OnDelete(DeleteBehavior.Cascade);
		}
    }
}
