using Microsoft.Data.SqlClient;
using Pronia.DataAccesLayer;

namespace Pronia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ProniaContext>();
            var app = builder.Build();
             
            app.UseStaticFiles();
            app.MapControllerRoute("areas", "{area:exists}/{controller=Slider}/{action=Index}/{id?}");       
            app.MapControllerRoute("defult", "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
