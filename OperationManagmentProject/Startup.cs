// Startup.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using OperationManagmentProject.Data;
using OperationManagmentProject.Services.User;
using OperationManagmentProject.SocketConnection;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        services.AddControllersWithViews();
        services.AddSignalR();
        services.AddDirectoryBrowser();
        services.AddScoped<IUserService, UserService>();
        services.AddSwaggerGen();
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

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });
        app.UseHttpsRedirection();
        //app.UseStaticFiles(); I may need this when import the cities but now comment because on the the use below for images
        app.UseRouting();
        app.UseDirectoryBrowser();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider("C:\\uploads"),
            RequestPath = "/uploads"
        });
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "User",
                pattern: "{controller=User}/{action=Index}/{id?}");
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "University",
                pattern: "{controller=University}/{action=Index}/{id?}");
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "Image",
                pattern: "{controller=Image}/{action=Index}/{id?}");
        });
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<SocketHub>("/socketHub");
        });
    }
}
