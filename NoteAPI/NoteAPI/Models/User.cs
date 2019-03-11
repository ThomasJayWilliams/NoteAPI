using System.ComponentModel.DataAnnotations;

namespace NoteAPI.Models
{
	public class User
	{
		[Key]
		public int ID { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string SecondName { get; set; }
	}
}
