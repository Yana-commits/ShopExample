using FruitShop.Host.Services.Interfaces;
using FruitShop.Host.Services;
using Order.Host.Data;
using Order.Host.Repositories;
using Order.Host.Repositories.Interfaces;
using Order.Host.Services;

var configurations = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
}).AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "eShop- Order Http Api",
        Version = "v1",
        Description = "The Order Service Http Api"
    });

    var authority = configurations["Authorization:Authority"];
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows()
        {
            Implicit = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                TokenUrl = new Uri($"{authority}/connect/token"),
                Scopes = new Dictionary<string, string>()
                {
                    {"basket.basketCache.api","basket.basketCache.api"},
                    { "order.order.api", "orderapi" },
                    { "order.orderbff.api", "orderbffapi" }
                }
            }
        }
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.AddConfiguration();

builder.Services.Configure<Config>(configurations);
builder.Services.AddHttpClient();

builder.Services.AddAuthorization(configurations);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IOrderItemRepository, OrderItemRepository>();

builder.Services.AddTransient<IInternalHttpClientService, InternalHttpClientService>();

builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IOrderItemService, OrderItemService>();

builder.Services.AddDbContextFactory<ApplicationDbContext>(options => options.UseNpgsql(configurations["ConnectionString"]));
builder.Services.AddScoped<IDbContextWrapper<ApplicationDbContext>, DbContextWrapper<ApplicationDbContext>>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        builder => builder
        .SetIsOriginAllowed((host) => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint($"{configurations["BasePath"]}/swagger/v1/swagger.json", "Order.Api V1");
        setup.OAuthClientId("orderswaggerui");
        setup.OAuthAppName("Order Swagger UI");
    });

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();



app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapControllers();
});

CreateDbIfNoExist(app);
app.Run();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
    return builder.Build();
}

void CreateDbIfNoExist(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}
