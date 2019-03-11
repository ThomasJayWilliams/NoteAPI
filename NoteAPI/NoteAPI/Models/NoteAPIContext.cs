using Microsoft.EntityFrameworkCore;

namespace NoteAPI.Models
{
	public class NoteAPIContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Note> Notes { get; set; }

		public NoteAPIContext(DbContextOptions<NoteAPIContext> options) : base(options)
		{
			Database.EnsureCreated();
		}
	}
}
