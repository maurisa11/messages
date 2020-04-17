using MessageBoard.Hubs;
using MessageBoard.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MessageBoard.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessagesController : ControllerBase
	{
		private readonly IMessageService messageService;
		private IHubContext<MessageHub> messageHub;

		public MessagesController(IMessageService messageService,
			IHubContext<MessageHub> messageHub)
		{
			this.messageService = messageService;
			this.messageHub = messageHub;
		}

		[HttpGet]
		public JsonResult Get()
		{
			var messages = messageService.GetMessages();
			return new JsonResult(messages);
		}

		[HttpPost("[action]")]
		public JsonResult Create([FromBody]string text)
		{
			var newMessage = messageService.CreateMessage(text);
			messageHub.Clients.All.SendAsync("MessageReceived", newMessage);
			return new JsonResult(newMessage != null);
		}
	}
}