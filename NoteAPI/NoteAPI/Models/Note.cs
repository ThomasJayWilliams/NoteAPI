using System.ComponentModel.DataAnnotations;

namespace NoteAPI.Models
{
	public class Note
	{
		[Key]
		public int ID { get; set; }
		public string Name { get; set; }
		public string Content { get; set; }
		public int UserID { get; set; }
	}
}
