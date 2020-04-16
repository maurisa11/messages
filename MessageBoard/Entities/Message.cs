using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Entities
{
	public class Message : GenericEntity
	{
		public string Text { get; set; }
	}
}
