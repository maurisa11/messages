using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageBoard.DataAccess.UnitOfWorks;
using MessageBoard.Entities;

namespace MessageBoard.Services
{
	public interface IMessageService
	{
		Message CreateMessage(string text);

		IEnumerable<Message> GetMessages();
	}

	public class MessageService : IMessageService
	{
		private readonly MessageUnitOfWork unitOfWork;
		public MessageService(MessageUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;

		}
		public Message CreateMessage(string text)
		{
			var message = new Message()
			{
				CreatedDate = DateTime.Now,
				Text = text
			};

			if (unitOfWork.MessageRepository.Insert(message))
				return message;

			return null;
		}
		public IEnumerable<Message> GetMessages()
		{
			return unitOfWork.MessageRepository
				.GetAll(null, 
				o => o.OrderByDescending(x => x.CreatedDate));
		}
	}

}
