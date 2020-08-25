using Caliburn.Micro;
using Retailer.Core.Helpers; // Should know nothing about core
using Retailer.Core.Models; // Should know nothing about core
using Retailer.Desktop.UI.Helpers;
using Retailer.Desktop.UI.Services;
using Retailer.Desktop.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Retailer.Desktop.UI
{
    public class Bootstrapper : BootstrapperBase
    {
        // DI: handles instantiation of classes
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();

            ConventionManager.AddElementConvention<PasswordBox>(
            PasswordBoxHelper.BoundPasswordProperty,
            "Password",
            "PasswordChanged");
        }

        protected override void Configure()
        {
            _container.Instance(_container)
                .PerRequest<IProductService, ProductService>();

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<IUserModel, UserModel>()
                .Singleton<IApiHelper, ApiHelper>();

            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(vmType => _container.RegisterPerRequest(
                    vmType, 
                    vmType.ToString(), 
                    vmType));
        }

        protected override void OnStartup(object sender, StartupEventArgs e) => DisplayRootViewFor<ShellViewModel>();

        protected override object GetInstance(Type service, string key) => _container.GetInstance(service, key);

        protected override IEnumerable<object> GetAllInstances(Type service) => _container.GetAllInstances(service);

        protected override void BuildUp(object instance) => _container.BuildUp(instance);
    }
}
