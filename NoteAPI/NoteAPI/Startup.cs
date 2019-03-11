using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NoteAPI.Services;
using NoteAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace NoteAPI
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
			string con = "Server=(localdb)\\mssqllocaldb;Database=noteapistorage;Trusted_Connection=True;MultipleActiveResultSets=true";
			services.AddDbContext<NoteAPIContext>(options => options.UseSqlServer(con));
			services.AddSingleton<TokenService>();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info() { Title = "NoteAPI", Version = "1.0" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();

			app.UseMvc();

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("./swagger/v1/swagger.json", "NoteAPI");
			});
		}
	}
}
