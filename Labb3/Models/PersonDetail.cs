using System;
namespace Labb3.Models
{
	public class PersonDetail
	{
		public PersonDetail() {}

		public int Id { get; set; }
		public String Name { get; set; }
		public int Age { get; set; }
		public int FavSongId { get; set; }
		public String FavSong { get; set; }
		public String Artist { get; set; }
    }
}

