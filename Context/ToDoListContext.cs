using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoListWebApp.Models;

namespace ToDoListWebApp.Context
{
	public sealed class ToDoListContext: DbContext
	{
		public DbSet<User> Users { get; set; }
		public ToDoListContext(DbContextOptions<ToDoListContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}
    }
}
