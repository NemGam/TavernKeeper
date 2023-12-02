using DnDManager.Services;
using DnDManager.ViewModels;

namespace DnDManager.Commands
{
    /// <summary>
    /// Command that allows navigation into different ViewModels.
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel to navigate into.</typeparam>
    internal class NavigateCommand<TViewModel> : CommandBase
        where TViewModel : ViewModelBase
    {
        private readonly NavigationService<TViewModel> _navigationService;


        public NavigateCommand(NavigationService<TViewModel> navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}
