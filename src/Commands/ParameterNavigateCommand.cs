using DnDManager.Services;
using DnDManager.ViewModels;

namespace DnDManager.Commands
{
    /// <summary>
    /// Command that allows navigation into different ViewModels with parameters.
    /// <typeparam name="TParameter">Parameter to pass.</typeparam>
    /// <typeparam name="TViewModel">ViewModel to navigate into.</typeparam>
    ///  </summary>
    internal class ParameterNavigateCommand<TParameter, TViewModel> : CommandBase
        where TViewModel : ViewModelBase
    {
        private readonly ParameterNavigationService<TParameter, TViewModel> _navigationService;
        private readonly TParameter? _parameter;

        public ParameterNavigateCommand(ParameterNavigationService<TParameter, TViewModel> navigationService, TParameter parameter)
        {
            _navigationService = navigationService;
            _parameter = parameter;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.Navigate(_parameter);
        }
    }
}
