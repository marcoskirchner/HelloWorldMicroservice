using System;
using HelloWorldMicroservice.Configs;
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
                        InstanceId = $"{Environment.MachineName}-{Guid.NewGuid().ToString().Split('-')[0]}",
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
                        case "Kafka":
                            services.Configure<KafkaConfig>(config.GetSection("KafkaConfig"));
                            services.AddSingleton<IHelloWorldReceiver, KafkaHelloWorldReceiver>();
                            services.AddSingleton<IHelloWorldSender, KafkaHelloWorldSender>();
                            break;
                        case "RabbitMq":
                            services.Configure<RabbitMqConfig>(config.GetSection("RabbitMqConfig"));
                            services.AddSingleton<IHelloWorldReceiver, RabbitMqHelloWorldReceiver>();
                            services.AddSingleton<IHelloWorldSender, RabbitMqHelloWorldSender>();
                            services.AddSingleton<MessageSerializer>();
                            break;
                    }

                    services.AddHostedService<ProducerService>();
                    services.AddHostedService<ConsumerService>();
                });
    }
}
