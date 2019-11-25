﻿using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Catalog.Wpf.Commands;
using Catalog.Wpf.Comparers;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Wpf.ViewModel
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private ObservableCollection<GameViewModel> games = new ObservableCollection<GameViewModel>();
        private string searchTerm;
        private MainWindowViewMode viewMode = MainWindowViewMode.GalleryMode;
        private ICommand editGameCommand;
        private ICommand deleteGameCommand;

        public MainWindowViewModel()
        {
            RefreshGames = new DelegateCommand(_ => RefreshGamesCollection());

            RefreshGamesCollection();

            FilteredGames = new ListCollectionView(Games)
            {
                CustomSort = new GameComparer(),
                Filter = obj =>
                {
                    if (obj is GameViewModel game)
                    {
                        return game.Title.IndexOf(SearchTerm ?? string.Empty,
                                   StringComparison.InvariantCultureIgnoreCase) >= 0;
                    }

                    return false;
                }
            };

            PropertyChanged += RefreshFilteredGames;
        }

        public ListCollectionView FilteredGames { get; }

        public ICommand RefreshGames { get; }

        public ObservableCollection<GameViewModel> Games
        {
            get => games;
            set
            {
                if (Equals(value, games)) return;
                games = value;
                OnPropertyChanged();
            }
        }

        public string SearchTerm
        {
            get => searchTerm;
            set
            {
                if (value == searchTerm) return;
                searchTerm = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewMode ViewMode
        {
            get => viewMode;
            set
            {
                if (Equals(value, viewMode)) return;
                viewMode = value;
                OnPropertyChanged();
            }
        }

        public ICommand EditGameCommand =>
            editGameCommand ??= new EditGameCommand(this);

        public ICommand DeleteGameCommand =>
            deleteGameCommand ??= new DeleteGameCommand(this);

        public ICommand ChangeViewModeCommand => new DelegateCommand(mode => { ViewMode = (MainWindowViewMode) mode; });

        public void RefreshGamesCollection()
        {
            var db = Application.Current.Database();

            var updatedGames = db.Games
                .Include(g => g.Items)
                .Select(gc => new GameViewModel(gc));

            Games.Clear();

            foreach (var game in updatedGames)
            {
                Games.Add(game);
            }
        }

        private void RefreshFilteredGames(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SearchTerm))
            {
                return;
            }

            FilteredGames.Refresh();
        }
    }
}
