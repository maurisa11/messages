using MessageBoard.DataAccess.UnitOfWorks;
using MessageBoard.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard
{
	public static class DIContainer
	{
		public static void AddInjectedDependencies(this IServiceCollection services)
		{
			services.AddTransient<MessageUnitOfWork>();
			services.AddTransient<IMessageService, MessageService>();
		}
	}



}
