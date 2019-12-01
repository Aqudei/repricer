using Caliburn.Micro;
using Repricer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
