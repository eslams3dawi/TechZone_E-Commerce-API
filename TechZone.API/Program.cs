using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using TechZone.API.Middleware;
using TechZone.BLL.AutoMapper;
using TechZone.BLL.Services.AccountService;
using TechZone.BLL.Services.CategoryService;
using TechZone.BLL.Services.Email;
using TechZone.BLL.Services.OrderService;
using TechZone.BLL.Services.PaymentService;
using TechZone.BLL.Services.ProductServices;
using TechZone.BLL.Services.ShoppingCartService;
using TechZone.DAL.Database;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.CategoryRepo;
using TechZone.DAL.Repository.OrderRepo;
using TechZone.DAL.Repository.PaymentRepo;
using TechZone.DAL.Repository.ProductRepo;
using TechZone.DAL.Repository.ShoppingCartRepo;

namespace TechZone.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Register DbContext
            builder.Services.AddDbContext<TechZoneContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //Register Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<TechZoneContext>()
                .AddDefaultTokenProviders();

            //Register Auto Mapper
            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));

            //Register JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                // Set the default scheme that the app uses to authenticate users
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                // Set the default scheme used when the app needs to challenge the user (when token is missing or invalid)
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                // Get the secret key from configuration to use in verifying JWT signatures
                var securityKeyString = builder.Configuration.GetSection("SecretKey").Value;

                // Convert the secret key string to a byte array
                var securityKeyByte = Encoding.ASCII.GetBytes(securityKeyString);

                // Create a SymmetricSecurityKey from the byte array to use for signing the token
                SecurityKey securityKey = new SymmetricSecurityKey(securityKeyByte);

                // Configure the rules for validating incoming JWT tokens
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero //No extra time for token if it valid
                };
            });

            //Register IMemoryCaching
            builder.Services.AddMemoryCache();

            //Stripe
            Stripe.StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Value;

            //Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                 connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                 sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions()
                 {
                     TableName = "Logs",
                     AutoCreateSqlTable = true
                 }
                ).CreateLogger();

            builder.Host.UseSerilog();

            //Email SMTP
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            //=====================//
            //Register Classes
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.AddTransient<IEmailService, EmailService>();

            //=====================//
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ErrorHandlerException>();

            app.UseHttpsRedirection();

            // Middleware that checks for a valid token in each request before accessing protected endpoints
            app.UseAuthentication();
            // Middleware that checks if the authenticated user has the right permissions (roles, policies)
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
