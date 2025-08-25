using DnDManager.Commands;
using DnDManager.Models;
using DnDManager.Services;
using DnDManager.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager.ViewModels
{
    internal class CreateGameViewModel : ViewModelBase
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }



        private string? _campaignName;
        public string? CampaignName
        {
            get => _campaignName;
            set
            {
                _campaignName = value;
                OnPropertyChanged(nameof(CampaignName));
            }
        }
        public NavigateCommand<MainPlayerViewModel> GoBackCommand { get; }
        public CreateGameCommand CreateGameCommand { get; }
        public CreateGameViewModel(DatabaseProvider dbProvider, UserStore userStore,
            NavigationService<MainPlayerViewModel> mainPlayerViewModelNS)
        {
            GoBackCommand = new NavigateCommand<MainPlayerViewModel>(mainPlayerViewModelNS);
            CreateGameCommand = new CreateGameCommand(this, userStore, dbProvider, null, mainPlayerViewModelNS);
        }
    }
}
