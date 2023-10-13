using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using StoreAPI.Models;

namespace StoreAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Member>("Members");
            modelBuilder.EntitySet<Category>("Categories");
            modelBuilder.EntitySet<Product>("Products");
            modelBuilder.EntitySet<Order>("Orders");
            modelBuilder.EntitySet<OrderDetail>("OrderDetails");

            builder.Services.AddDbContext<eStoreContext>();

            builder.Services.AddControllers().AddOData(
                options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
                    "odata",
                    modelBuilder.GetEdmModel()));

            var app = builder.Build();
            app.UseODataRouteDebug();
            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.Run();
        }
    }
}