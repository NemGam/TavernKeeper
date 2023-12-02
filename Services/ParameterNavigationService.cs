using DnDManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager.Services
{
    /// <summary>
    /// Class that creates the new ViewModel and navigates into it with parameter.
    /// </summary>
    /// <typeparam name="TParameter">Parameter to pass.</typeparam>
    /// <typeparam name="TViewModel">ViewModel to navigate into.</typeparam>
    class ParameterNavigationService<TParameter, TViewModel>
        where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TParameter, TViewModel> _createViewModel;

        public ParameterNavigationService(NavigationStore navigationStore, Func<TParameter, TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        /// <summary>
        /// Navigate into the stored ViewModel.
        /// </summary>
        /// <param name="parameter">Parameter to pass.</param>
        public void Navigate(TParameter? parameter)
        {
            _navigationStore.CurrentViewModel = _createViewModel(parameter);
        }
    }
}
