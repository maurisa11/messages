using MessageBoard.DataAccess.Base;
using MessageBoard.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.DataAccess.UnitOfWorks
{
	public class MessageUnitOfWork : UnitOfWorkBase
	{
		private IGenericRepository<Message> messageRepository;

		public MessageUnitOfWork(MessageContext context) 
			: base(context)
		{
		}

		public IGenericRepository<Message> MessageRepository
		{
			get
			{
				if (messageRepository == null)
				{
					messageRepository = new GenericRepository<Message>(Context);
				}
				return messageRepository;
			}
		}
	}

}
