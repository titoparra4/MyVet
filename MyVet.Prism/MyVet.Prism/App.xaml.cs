using Prism;
using Prism.Ioc;
using MyVet.Prism.ViewModels;
using MyVet.Prism.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyVet.Common.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MyVet.Prism
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTUyMzA4QDMxMzcyZTMzMmUzMFRJOENpWXhsL2JCeHFic1VUZWxIQmpoVlhiTTRnYnRod1FvbUsyU2l4TTQ9");

            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<PetsPage, PetsPageViewModel>();
            containerRegistry.RegisterForNavigation<PetPage, PetPageViewModel>();
            containerRegistry.RegisterForNavigation<HistoriesPage, HistoriesPageViewModel>();
            containerRegistry.RegisterForNavigation<HistoryPage, HistoryPageViewModel>();
            containerRegistry.RegisterForNavigation<PetTabbedPage, PetTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<VeterinaryMasterDetailPage, VeterinaryMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<AgendaPage, AgendaPageViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
        }
    }
}
