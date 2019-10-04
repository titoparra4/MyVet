using MyVet.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyVet.Prism.ViewModels
{
	public class PetPageViewModel : ViewModelBase
	{
        private PetResponse _pet;
        public PetPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            
        }

        public PetResponse Pet
        {

            get => _pet;
            set => SetProperty(ref _pet, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("pet"))
            {
                Pet = parameters.GetValue<PetResponse>("pet");
                Title = Pet.Name;
            }
        }
    }
}
