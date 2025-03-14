using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pursue.Extension.Cryptologys;

namespace Pursue.Extension.DependencyInjection
{
    public static class CryptologysDependencyInjection
    {
        public static IServiceCollection AddCryptology(this IServiceCollection services)
        {
            services.TryAddSingleton<IUserRsaCode, UserRsaCode>();
            services.TryAddSingleton<IUserVerifyCode, UserVerifyCode>();

            return services;
        }
    }
}
