
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CompanyEmployees.IDP;

internal static class HostingExtensions
{
   

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();
        
        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

        builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
        })
            .AddDeveloperSigningCredential()           
            .AddTestUsers(TestUsers.Users)
            .AddConfigurationStore(opt =>
            {
                opt.ConfigureDbContext = c => c.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"), sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(opt =>
            {
                opt.ConfigureDbContext = o => o.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"), sql => sql.MigrationsAssembly(migrationsAssembly));
            });



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
        app.UseAuthorization();

        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
