using CompanyEmployees.IDP.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CompanyEmployees.IDP.InitialSeed
{
    public class SeedUserData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<UserContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddIdentity<User, IdentityRole>(o => { o.Password.RequireDigit = false; o.Password.RequireNonAlphanumeric = false; o.Password.RequiredLength = 3; o.Password.RequireUppercase = false; })
                .AddEntityFrameworkStores<UserContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    CreateUser(scope, "alice", "smith", "Alice Alice's Avenue 214", "USA", "1", "alice", "Administrator", "AliceSmith@email.com");

                    CreateUser(scope, "bob", "smith", "Bob Bob's Boulevard 215", "USA", "2", "bob", "Visitor", "BobSmith@email.com");
                }
            }
        }

        private static void CreateUser(IServiceScope scope, string name, string lastName, string address, string country, string id, string password, string role, string email)
        {
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var user = userMgr.FindByNameAsync(email).Result;
            if(user == null)
            {
                user = new User
                {
                    UserName = email,
                    Email = email,
                    Id = id,
                    FirstName = name,
                    LastName = lastName,
                    Address = address,
                    Country = country
                };

                var result = userMgr.CreateAsync(user, password).Result;
                CheckResult(result);

                result = userMgr.AddToRoleAsync(user, role).Result;
                CheckResult(result);

                result = userMgr.AddClaimsAsync(user, new Claim[]
                {
                    new Claim(JwtClaimTypes.GivenName, user.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, user.LastName),
                    new Claim(JwtClaimTypes.Role, role),
                    new Claim(JwtClaimTypes.Address, user.Address),
                    new Claim("country", user.Country)
                }).Result;
                CheckResult(result);

            }

            
        }
        private static void CheckResult(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
        }
    }
}
