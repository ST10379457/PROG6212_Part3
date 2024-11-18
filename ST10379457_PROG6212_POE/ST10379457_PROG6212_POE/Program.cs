namespace ST10379457_PROG6212_POE
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseSession();

            app.MapRazorPages();

            app.Run();
        }
    }
}
//(Microsoft Learn, 2023).

/*
 * Microsoft Learn. 2023. Introduction to Razor Pages in ASP.NET Core, 7 October 2023. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-8.0&tabs=visual-studio [Accessed 21 May 2024].
 * Microsoft Learn. 2023. Access HttpContext in ASP.NET Core, 24 October 2023. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-8.0&tabs=visual-studio [Accessed 24 May 2024].
 */