using Caliburn.Micro;
using Retailer.Desktop.UI.Events.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Desktop.UI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LoggedInEventModel>
    {
        private SimpleContainer _container;
        private IEventAggregator _events;
        private SalesViewModel _salesVM;

        public ShellViewModel(
            SimpleContainer container,
            IEventAggregator events,
            SalesViewModel salesVM)
        {
            _container = container;
            _events = events;            
            _salesVM = salesVM;

            _events.Subscribe(this); // 1. ShellViewModel is subscribed to LoggedInEventModel
            ActivateItem(_container.GetInstance<LoginViewModel>());            
        }

        // 2. Called once a LoggedInEventModel event is Broadcast
        public void Handle(LoggedInEventModel message)
        {
            // 'Redirect' to SalesViewModel
            ActivateItem(_salesVM);
        }
    }
}
