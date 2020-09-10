using Caliburn.Micro;
using Retailer.Core.Helpers;
using Retailer.Core.Models;
using Retailer.Desktop.UI.Events.Models;

namespace Retailer.Desktop.UI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LoggedInEventModel>
    {
        private IApiHelper _apiHelper;
        private SimpleContainer _container;
        private IEventAggregator _events;
        private SalesViewModel _salesVM;
        private IUserModel _loggedInUser;

        public ShellViewModel(
            IApiHelper apiHelper,
            SimpleContainer container,
            IEventAggregator events,
            SalesViewModel salesVM,
            IUserModel loggedInUser)
        {
            _apiHelper = apiHelper;
            _container = container;
            _events = events;            
            _salesVM = salesVM;
            _loggedInUser = loggedInUser;

            _events.Subscribe(this); // 1. ShellViewModel is subscribed to LoggedInEventModel
            ActivateItem(_container.GetInstance<LoginViewModel>());            
        }

        // 2. Called once a LoggedInEventModel event is Broadcast
        public void Handle(LoggedInEventModel message)
        {
            // 'Redirect' to SalesViewModel
            ActivateItem(_salesVM);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public bool IsLoggedIn
        {
            get 
            {
                if (!string.IsNullOrWhiteSpace(_loggedInUser.Token))
                    return true;

                return false;
            }
        }

        public void Logout()
        {
            // Clear API Client
            _apiHelper.ClearClient();
            // Clear logged-in user
            _loggedInUser.Logout();
            // Activate login view
            ActivateItem(_container.GetInstance<LoginViewModel>());

            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void ExitApp()
        {
            // Clear logged-in user
            _loggedInUser.Logout();
            TryClose();
        }
    }
}
