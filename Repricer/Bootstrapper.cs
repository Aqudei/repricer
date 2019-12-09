using Caliburn.Micro;
using Repricer.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using MahApps.Metro.Controls.Dialogs;
using Repricer.Models;
using Unity;

namespace Repricer
{
    class Bootstrapper : BootstrapperBase
    {
        private readonly IUnityContainer _unityContainer = new Unity.UnityContainer();
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void BuildUp(object instance)
        {
            _unityContainer.BuildUp(instance);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _unityContainer.ResolveAll(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            return _unityContainer.Resolve(service, key);
        }

        protected override void Configure()
        {
            _unityContainer.RegisterSingleton<IWindowManager, WindowManager>();
            _unityContainer.RegisterSingleton<IEventAggregator, EventAggregator>();
            _unityContainer.RegisterInstance(DialogCoordinator.Instance);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FBAInventoryItem, InventoryItem>()
                    .ForMember(m => m.Sku, opts => opts.MapFrom(source => source.SellerSku));

            });
            _unityContainer.RegisterInstance(config.CreateMapper());

            // Database.SetInitializer(new MigrateDatabaseToLatestVersion<RepricerContext, Migrations.Configuration>());
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
