using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AcoStand.Data;
using Microsoft.Extensions.Hosting;

namespace AcoStand {
    public class Program {
        public static void Main(string[] args) {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Cria a password por defeito das contas criadas no sistema através da Seed 
                    string passwords = "123Qwe.";


                    // Chama o método Initialize que irá tratar de Criar os utilizadores, passando-lhe a password por parâmetro
                    DbInitializer.Initialize(services, passwords).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ocorreu um erro a criar a Base de Dados");
                }
            }

            host.Run();
        }

        //private static object CreateWebHostBuilder(string[] args)
        //{
        //    throw new NotImplementedException();
        //}

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
