using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Sqlite;
using TaskManagerApp.Data; // Ensure this matches your project's namespace

namespace TaskManagerApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure your DbContext
            services.AddDbContext<TaskDbContext>(options =>
                options.UseSqlite("Data Source=/tmp/taskdb.db"));

            services.AddControllersWithViews(); // Add MVC support
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Tasks}/{action=Index}/{id?}"); // Set default routing
            });
        }
    }
}
