﻿using MessageBoard.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MessageBoard.DataAccess.Base
{
	public class MessageContext : DbContext
	{
		public MessageContext(DbContextOptions<MessageContext> options)
	   : base(options)	{ }

		public DbSet<Message> Messages { get; set; }
	}
}
