using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using TARge25shop.ApplicationServices.Services;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;
using TARge25Shop.SpaceshipTest.Macros;
using TARge25Shop.SpaceshipTest.Mock;

namespace TARge25Shop.SpaceshipTest
{
    public abstract class TestBase
    {

        protected IServiceProvider serviceProvider { get; set; }

        protected TestBase()
        {
            var services = new ServiceCollection();
            SetupServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        // Seame üles testide läbiviimiseks vajalikud teenused mujalt projektist
        // See meetod annab ka mälusoleva andmebaasi mida testideks kasutada,
        // toimib kui program.cs-i sisu testide jooksutamiseks, ent lühidal kujul
        // tühi ServiceCollection-tüüpi muutuja kuhu asetame teenused

        public virtual void SetupServices(ServiceCollection services)
        {
            services.AddScoped<ISpaceshipServices, SpaceshipServices>();
            services.AddScoped<IFileServices, FileServices>();
            services.AddScoped<IHostEnvironment, MockIHostEnvironment>();

            services.AddDbContext<TARge25ShopContext>(
                x =>
                {
                    x.UseInMemoryDatabase("TEST");
                    //vaigistame errorid (kui adnmebaasi CRUD ei toimi, siis DB errorit ei anna)
                    x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                }
                );
            RegisterMacros(services);
        }

        public void Dispose()
        {

        }

        // Summary
        // Leia üles kindel teenus, teenusepakkujalt.
        // serviceProvider omab teenuseid, GetService hangib X tüüpi teenuse,
        // C# on ükskõik, mis tüüpi võimalik ilma tüübita näidata tähe "T"-ga ehk "Template"
        // Teenuse tüüp

        protected T Svc<T>()
        {
            return serviceProvider.GetService<T>();
        }
        // Registreerib macrodest teenuseid kui nad ei ole liidesed ja ei ole abstraktsed
        // On vaja testi setupide seadistuseks
        // Makro --> Teenus
        // param name="services" Teenused, kuhu lisab makrodest muid teenuseid

        private void RegisterMacros(ServiceCollection services)
        {
            var macroBaseType = typeof(IMacros); // error

            var macros = macroBaseType.Assembly.GetTypes()
                .Where(t => macroBaseType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            foreach (var macro in macros)
            {
                services.AddSingleton(macro);
            }
        }
    }
}
