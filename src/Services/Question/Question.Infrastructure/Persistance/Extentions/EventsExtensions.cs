using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Infrastructure.Persistance.Extentions
{
    public static class EventsExtensions
    {
        //public static IServiceCollection AddEventBusCustom(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var config = new EventBusSettings();
        //    configuration.Bind("EventBus", config);
        //    services.AddSingleton(config);

        //    ConnectionFactory factory = new ConnectionFactory
        //    {
        //        HostName = config.HostName,
        //        UserName = config.User,
        //        Password = config.Password
        //    };

        //    services.AddSingleton(factory);

        //    return services;
        //}
    }
}
