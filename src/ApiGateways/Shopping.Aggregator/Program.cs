using Shopping.Aggregator.Services;

namespace Shopping.Aggregator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient<ICatalogService, CatalogService>(opt =>
            {
                opt.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]);
            });

            builder.Services.AddHttpClient<IBasketService, BasketService>(opt =>
            {
                opt.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]);
            });

            builder.Services.AddHttpClient<IOrderService, OrderService>(opt =>
            {
                opt.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
