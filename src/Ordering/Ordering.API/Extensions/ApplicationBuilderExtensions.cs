using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.API.Consumers;

namespace Ordering.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static EventBusConsumer Listener { get; set; }

        public static IApplicationBuilder UseEventBusListner(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<EventBusConsumer>();
            var lifeCycle = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            lifeCycle.ApplicationStarted.Register(OnStarted);
            lifeCycle.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            Listener.Consume();
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }
    }
}
