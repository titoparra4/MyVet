using MyVet.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyVet.Prism.ViewModels
{

    
	public class PetsPageViewModel : ViewModelBase
	{
        private readonly INavigationService _navigationService;
        private OwnerResponse _owner;
        private ObservableCollection<PetItemViewModel> _pets;

        public PetsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Pets";
            _navigationService = navigationService;
        }

        public ObservableCollection<PetItemViewModel> Pets
        {
            get => _pets;
            set => SetProperty(ref _pets, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if(parameters.ContainsKey("owner"))
            {
                _owner = parameters.GetValue<OwnerResponse>("owner");
                Title = $"Pets of: {_owner.FullName}";
                Pets = new ObservableCollection<PetItemViewModel>(_owner.Pets.Select(p => new PetItemViewModel(_navigationService)
                {
                    Born = p.Born,
                    Histories = p.Histories,
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    Name = p.Name,
                    PetType = p.PetType,
                    Race = p.Race,
                    Remarks = p.Remarks
                }).ToList());
            }
        }
    }
}
