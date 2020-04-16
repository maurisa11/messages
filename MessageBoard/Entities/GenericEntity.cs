using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Entities
{
	public abstract class GenericEntity
	{
		[Key, Column(Order = 0)]
		public int Id { get; set; }

		public DateTime CreatedDate { get; set; }
	}
}
