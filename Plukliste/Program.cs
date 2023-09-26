using Plukliste.BLL.Filehandler;
using Plukliste.BLL.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Plukliste.BLL.ManualPrinter;
using Plukliste.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Plukliste;

class PluklisteProgram
{
    static void Main()
    {
        using IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                services.AddSingleton<IFileHandler, XMLFileHandler>();
                services.AddSingleton<IFileHandler, CSVFileHandler>();
                services.AddSingleton<IManualPrinter, PluklisteManualPrinter>();
                services.AddSingleton<IItemRepository, SQLItemRepository>();
                services.AddDbContext<PluklistDbContext>(pdbc => pdbc.UseSqlServer(connectionString), ServiceLifetime.Singleton);
                services.AddDbContextFactory<PluklistDbContext>(pdbcf => pdbcf.UseSqlServer(connectionString), ServiceLifetime.Singleton);
                services.AddSingleton<PluklisteConsole>();
            })
            .Build();
       
        var pluklistConsole = host.Services.GetService<PluklisteConsole>();
        pluklistConsole.StartConsoleLoop();

        host.Run();
    }
}