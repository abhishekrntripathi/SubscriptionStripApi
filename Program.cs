
using Stripe;
using SubscriptionSystem.Service.StripeService;

namespace SubscriptionSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();  
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<IStripeService, StripeService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<SubscriptionService>();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
