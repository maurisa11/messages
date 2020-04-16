using MessageBoard.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessageBoard.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessagesController : ControllerBase
	{
		private readonly IMessageService messageService;

		public MessagesController(IMessageService messageService)
		{
			this.messageService = messageService;
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
			var created = messageService.CreateMessage(text);

			return new JsonResult(created);
		}
	}
}