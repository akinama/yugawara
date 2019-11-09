﻿using System.Threading.Tasks;
using Grpc.Core;
using MagicOnion.Hosting;
using MagicOnion.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Yugawara
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var magicOnionHost = MagicOnionHost.CreateDefaultBuilder()
                .UseMagicOnion(new MagicOnionOptions(isReturnExceptionStackTraceInErrorDetail: true), new ServerPort("localhost", 12345, ServerCredentials.Insecure))
                .UseConsoleLifetime()
                .Build();

            var webHost = Host.CreateDefaultBuilder(args)
                .ConfigureServices(collection =>
                {
                    collection.AddSingleton<MagicOnionServiceDefinition>(magicOnionHost.Services.GetService<MagicOnionHostedServiceDefinition>().ServiceDefinition);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                        {
                            serverOptions.AllowSynchronousIO = true;
                        })
                        .UseUrls("http://localhost:5432")
                        .UseStartup<Startup>();
                })
                .Build();

            await Task.WhenAll(webHost.RunAsync(), magicOnionHost.RunAsync());
        }
    }
}
