using MessageBoard.DataAccess.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MessageBoard.Hubs;


namespace MessageBoard
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
			{
				builder
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials()
				.WithOrigins("http://localhost:4200");
			}));

			services.AddControllers();
			services.AddDbContext<MessageContext>(options =>
			options.UseSqlServer(
						Configuration.GetConnectionString("MessageBoard"),
						providerOptions => providerOptions.EnableRetryOnFailure()
						)
			);
			services.AddInjectedDependencies();
			services.AddSignalR();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseCors("CorsPolicy");

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHub<MessageHub>("/message");
			});

			app.UseSpa(spa =>
			{
				// To learn more about options for serving an Angular SPA from ASP.NET Core,
				// see https://go.microsoft.com/fwlink/?linkid=864501

				spa.Options.SourcePath = "MessageBoardClient";

				if (env.IsDevelopment())
				{
					spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
				}
			});

			UpdateDatabase(app);

			//app.UseSignalR(options =>
			//{
			//	options.MapHub<MessageHub>("/MessageHub");
			//});


		}

		private static void UpdateDatabase(IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices
				.GetRequiredService<IServiceScopeFactory>()
				.CreateScope())
			{
				using (var context = serviceScope.ServiceProvider.GetService<MessageContext>())
				{
					context.Database.EnsureCreated(); //.Migrate();
				}
			}
		}

	}
}
