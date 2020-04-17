using MessageBoard.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Hubs
{
	public class MessageHub : Hub
	{
		public async Task NewMessage(string msg)
		{
			await Clients.All.SendAsync("MessageReceived", msg);
		}
	}  

}
