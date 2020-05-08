using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerRepository.Interface;
using CustomerService.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CustomerManagementServiceApi
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
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<ICustomerService, CustomerService.Service.CustomerService>();
            services.AddScoped<ICustomerRepository, CustomerRepository.Repository.CustomerRepository>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //************************Google OAuth configuration**********************
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = Configuration["GoogleAuthentication:ClinetID"];
                options.ClientSecret = "GoogleAuthentication:ClientSecret";
            });
            //*************************************************************************
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
