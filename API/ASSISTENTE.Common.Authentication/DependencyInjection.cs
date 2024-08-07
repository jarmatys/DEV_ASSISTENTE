using System.Text;
using ASSISTENTE.Common.Authentication.Settings;
using ASSISTENTE.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ASSISTENTE.Common.Authentication
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCommonAuthentication<TSettings>(this IServiceCollection services)
            where TSettings : IAuthenticationSettings
        {
            var settings = services.GetSettings<TSettings, AuthenticationSettings>(x => x.Authentication);

            var bytes = Encoding.UTF8.GetBytes(settings.JwtSecret);
            
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(bytes),
                        ValidAudience = settings.ValidAudience,
                        ValidIssuer = settings.ValidIssuer
                    };
                });

            return services;
        }
    }
}