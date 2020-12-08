using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApp.ViewModels.Account
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Не указан Email")]
		public string Email { get; set; } = null!;

		[Required(ErrorMessage = "Не указан пароль")]
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;
	}
}
