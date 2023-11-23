using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using ConvertJsonToWord.Helper;
using ConvertJsonToWord.Utils;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace ConvertJsonToWord
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // 配置服務集合
            ServiceCollection services = new ServiceCollection();
            
            ConfigureServices(services);

            // 建立 ServiceProvider
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            json2WordForm json2WordForm = serviceProvider.GetRequiredService<json2WordForm>();


            // 運行應用程式
            Application.Run(json2WordForm);
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<json2WordForm>();
            services.AddScoped<SwaggerGenerator>();
            services.AddScoped<SpireDocHelper>();
            services.AddScoped<RemoveWords>();

        }
    }
}