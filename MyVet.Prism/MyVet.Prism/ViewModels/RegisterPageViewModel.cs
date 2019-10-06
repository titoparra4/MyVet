using System;
using System.Threading.Tasks;
using MyVet.Common.Helpers;
using Prism.Commands;
using Prism.Navigation;

namespace MyVet.Prism.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _registerCommand;

        public RegisterPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Register new user";
            IsEnabled = true;
        }

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(Register));

        public string Document { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void Register()
        {
            var isValid = await ValidateData();
            if (!isValid)
            {
                return;
            }
        }

        private async Task<bool> ValidateData()
        {
            if (string.IsNullOrEmpty(Document))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter a document.", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(FirstName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter a firsname.", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(LastName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter a lastname.", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(Address))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter an address.", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(Email) || !RegexHelper.IsValidEmail(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter a valid email.", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(Phone))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter a phone.", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(Password) || Password.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter a password at least 6 character.", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter a password confirm.", "Accept");
                return false;
            }

            if (!Password.Equals(PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert("Error", "The password and confirm does not match.", "Accept");
                return false;
            }

            return true;
        }
    }
}
