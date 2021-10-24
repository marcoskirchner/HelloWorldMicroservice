using System;
using HelloWorldMicroservice.Display;
using HelloWorldMicroservice.Messaging;
using HelloWorldMicroservice.Services;
using HelloWorldMicroservice.Timers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelloWorldMicroservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var config = hostContext.Configuration;
                    var serviceConfig = new ServiceConfig()
                    {
                        InstanceId = Environment.MachineName,
                    };
                    config.GetSection("ServiceConfig").Bind(serviceConfig);
                    services.AddSingleton(serviceConfig);

                    services.AddTransient<ITimer, Timer>();
                    services.AddTransient<IDisplay, ConsoleDisplay>();

                    switch (serviceConfig.MessagingImplementation)
                    {
                        case "InMemoryLocal":
                            services.AddSingleton<InMemoryLocalMessaging>();
                            services.AddSingleton<IHelloWorldReceiver>(p => p.GetService<InMemoryLocalMessaging>());
                            services.AddSingleton<IHelloWorldSender>(p => p.GetService<InMemoryLocalMessaging>());
                            break;
                    }

                    services.AddHostedService<ProducerService>();
                    services.AddHostedService<ConsumerService>();
                });
    }
}
