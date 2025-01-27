
using CompanyEmployees.IDP.Entities;
using EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CompanyEmployees.IDP;

internal static class HostingExtensions
{
   

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();
        builder.Services.AddAutoMapper(typeof(Program));

        var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        builder.Services.AddSingleton(emailConfig);
        builder.Services.AddScoped<IEmailSender, EmailSender>();

        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

        builder.Services.AddDbContext<UserContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("identitySqlConnection"));
        });

        builder.Services.AddIdentity<User, IdentityRole>(opt => { opt.Password.RequireDigit = false;opt.Password.RequiredLength = 7; opt.Password.RequireUppercase = false; })
            .AddEntityFrameworkStores<UserContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
        })
            .AddDeveloperSigningCredential()           
            
            .AddConfigurationStore(opt =>
            {
                opt.ConfigureDbContext = c => c.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"), sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(opt =>
            {
                opt.ConfigureDbContext = o => o.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"), sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddAspNetIdentity<User>();


        builder.Services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(2));


        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
       

        // uncomment if you want to add a UI
        app.UseStaticFiles();
       
        app.UseRouting();
       
        app.UseIdentityServer();

        // uncomment if you want to add a UI
        app.UseAuthentication();

        app.UseAuthorization();

        app.MapRazorPages();

        

        return app;
    }
}
