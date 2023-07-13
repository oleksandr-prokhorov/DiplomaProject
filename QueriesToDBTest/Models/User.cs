using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QueriesToDBTest.Models
{
	public class User
	{
		[Key]
		public string login { get; set; }
		[DataType(DataType.Password)]
		public string? password { get; set; }
		public string category { get; set; }
	}
}
