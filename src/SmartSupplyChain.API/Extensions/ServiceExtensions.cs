
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartSupplyChain.Application.Interfaces;
using SmartSupplyChain.Application.Services;
using SmartSupplyChain.Application.Validators;
using SmartSupplyChain.Core.Interfaces;
using SmartSupplyChain.Infrastructure.Data;
using SmartSupplyChain.Infrastructure.Data.Seed;
using SmartSupplyChain.Infrastructure.Repositories;
using SmartSupplyChain.Infrastructure.Security;

namespace SmartSupplyChain.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IShipmentRepository, ShipmentRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IDeliveryRepository, DeliveryRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<Application.Interfaces.IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IShipmentService, ShipmentService>();

        services.AddScoped<IDeliveryService, DeliveryService>();
        services.AddScoped<WarehouseService>();
        services.AddScoped<TrackingService>();
        services.AddScoped<PaymentService>();
        services.AddScoped<ReportService>();

        services.AddScoped<LoginValidator>();
        services.AddScoped<CustomerValidator>();
        services.AddScoped<OrderValidator>();
        services.AddScoped<ShipmentValidator>();

        services.AddScoped<JwtService>();
        services.AddScoped<PasswordHasher>();
        services.AddScoped<DatabaseSeeder>();

        var jwtKey = configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException(
                "JWT key is not configured.");
        }

        services.AddCors(options =>
        {
            options.AddPolicy("FrontendPolicy", policy =>
            {
                policy
                    .WithOrigins("http://127.0.0.1:5500")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtKey)),

                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.Zero
                    };
            });

        services.AddAuthorization();

        return services;
    }
}
