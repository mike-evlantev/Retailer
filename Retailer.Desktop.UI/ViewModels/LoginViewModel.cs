using Caliburn.Micro;
using Retailer.Core.Helpers;
using Retailer.Desktop.UI.Events.Models;
using System;
using System.Threading.Tasks;

namespace Retailer.Desktop.UI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private IApiHelper _apiHelper;
        private IEventAggregator _events;

        public LoginViewModel(
            IApiHelper apiHelper,
            IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        public string Username
        {
            get { return _username; }
            set 
            { 
                _username = value;
                NotifyOfPropertyChange(() => Username);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public string Password
        {
            get { return _password; }
            set 
            { 
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public bool CanLogin
        {
            get 
            {
                if (Username?.Length > 0 && Password?.Length > 0)
                    return true;

                return false;
            }
        }

        public bool IsErrorVisible
        {
            get
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                    return true;
                return false;
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set 
            { 
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }

        public async Task Login()
        {
            try
            {
                ErrorMessage = "";
                var user = await _apiHelper.AuthenticateAsync(Username, Password);
                await _apiHelper.GetLoggedInUserInfo(user.Token);

                // Broadcast
                _events.PublishOnUIThread(new LoggedInEventModel());


            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            
        }
    }
}
