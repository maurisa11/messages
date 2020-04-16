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
		bool CreateMessage(string text);

		IEnumerable<Message> GetMessages();
	}

	public class MessageService : IMessageService
	{
		private readonly MessageUnitOfWork unitOfWork;
		public MessageService(MessageUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;

		}
		public bool CreateMessage(string text)
		{
			var message = new Message()
			{
				CreatedDate = DateTime.Now,
				Text = text
			};

			return unitOfWork.MessageRepository.Insert(message);
		}
		public IEnumerable<Message> GetMessages()
		{
			return unitOfWork.MessageRepository.GetAll();
		}
	}

}
