using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using OfficeOpenXml;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            services.AddCors();
            services.AddHttpClient();
            services.AddScoped<ITokenService, TokenService>();
            services.Configure<OutlookMailSettings>(config.GetSection("OutlookMailSettings"));
            services.AddScoped<IMailService, MailService>();
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<LogUserActivity>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
