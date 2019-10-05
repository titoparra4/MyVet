using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyVet.Prism.ViewModels
{
	public class ProfilePageViewModel : ViewModelBase
	{
        public ProfilePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Profile";
        }
	}
}
