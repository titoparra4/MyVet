using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyVet.Prism.ViewModels
{
	public class AgendaPageViewModel : ViewModelBase
	{
        public AgendaPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Agenda";
        }
	}
}
