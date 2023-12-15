using System;
namespace Labb3.Models
{
	public class ViewModelPG
	{
		public IEnumerable<PersonGenres> PGList { get; set; }
		public IEnumerable<GenreDetail> GDList { get; set; }
	}
}

