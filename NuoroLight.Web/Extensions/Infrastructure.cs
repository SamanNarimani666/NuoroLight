using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using NuoroLight.Core.Convertors;

namespace NuoroLight.Web.Extensions;
public static class Infrastructure
{
    public static IServiceCollection RegisterServiceInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
        services.AddTransient<IViewRenderService, ViewRenderService>();
        return services;
    }
}