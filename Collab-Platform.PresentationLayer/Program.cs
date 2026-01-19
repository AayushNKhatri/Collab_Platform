using Collab_Platform.ApplicationLayer.DI;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using Collab_Platform.InfastructureLayer.Database;
using Collab_Platform.InfastructureLayer.DI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Serilog;
using Collab_Platform.PresentationLayer.Middleware;
using System.Reflection;
using Collab_Platform.ApplicationLayer.Helper;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var logConfig = new LogConfig();
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithProcessId()
            .Enrich.WithEnvironmentName()
            .WriteTo.Console()
            .WriteTo.File("logs/log.text", rollingInterval: RollingInterval.Day)
            .WriteTo.ApplicationInsights(
                builder.Configuration["ApplicationInsights:ConnectionString"],
                TelemetryConverter.Traces
            )
            // .WriteTo.PostgreSQL(    //Consume to much storege in db i am brkoe
            //     connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
            //     tableName:"logs",
            //     columnOptions:logConfig.GetColoumData(),
            //     needAutoCreateTable: true,
            //     useCopy:false
            //     
            //     )
            .MinimumLevel.Debug()
            .CreateLogger();

        // Add services to the container.
        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Web Api",
                Version = "v1"
            });

            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "Please enter token(No need to add Bearer)",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
             new OpenApiSecurityScheme
             {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id =  "Bearer"
                }
             },[]
        }
            });
        });
        builder.Host.UseSerilog();
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.RepoDI();
        builder.Services.ServiceDependencyInjecttion();
        builder.Services.AddOpenApi();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        builder.Services.AddIdentity<UserModel, IdentityRole>(opt =>
        {
            opt.Password.RequiredLength = 8;
            opt.User.RequireUniqueEmail = true;
            opt.Password.RequireNonAlphanumeric = false;
            opt.SignIn.RequireConfirmedEmail = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(
            options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["TokenSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["TokenSettings:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenSettings:Token"]!)),
                    ValidateIssuerSigningKey = true
                };
            });
        //builder.Services.AddScoped<ProjectAcessExtention>();
        builder.Services.AddAutoMapper(congif => congif.AddMaps(typeof(MapperConfig).Assembly));
        var app = builder.Build();
        app.UseMiddleware<LoggingMiddleware>();
        app.UseMiddleware<GlobalException>();
        using (var scope = app.Services.CreateScope())
        {
            var service = scope.ServiceProvider;
            var seedService = service.GetRequiredService<ISeedService>();
            await seedService.SeedRole();
            await seedService.SeedAdmin();
        }

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API v1");
        });

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSerilogRequestLogging();

        app.MapControllers();

        app.Run();
    }
}