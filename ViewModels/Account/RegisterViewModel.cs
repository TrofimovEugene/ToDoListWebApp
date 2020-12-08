using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.ViewModels.Account
{
	public class RegisterViewModel
	{
#pragma warning disable 8618
		public string FirstName { get; set; }

		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
		[Required(ErrorMessage = "Не указан Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Не указан пароль")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Пароль введен неверно")]
		public string ConfirmPassword { get; set; }
#pragma warning restore 8618
	}
}
