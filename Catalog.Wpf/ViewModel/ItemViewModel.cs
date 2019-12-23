﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Catalog.Model;
using Catalog.Wpf.Commands;
using Microsoft.Win32;

namespace Catalog.Wpf.ViewModel
{
    public class ItemViewModel : NotifyPropertyChangedBase
    {
        private ItemType? itemType;
        private bool missing;
        private Condition? condition;
        private string? conditionDetails;
        private string? notes;

        private ICommand? addFileCommand;
        private ICommand? removeFileCommand;
        private ICommand? addScanCommand;
        private ICommand? removeScanCommand;
        private ObservableCollection<ImageViewModel> scans = new ObservableCollection<ImageViewModel>();
        private ObservableCollection<FileViewModel> files = new ObservableCollection<FileViewModel>();

        public ItemType? ItemType
        {
            get => itemType;
            set
            {
                if (Equals(value, itemType)) return;
                itemType = value;
                OnPropertyChanged();
            }
        }

        public bool Missing
        {
            get => missing;
            set
            {
                if (value == missing) return;
                missing = value;
                OnPropertyChanged();
            }
        }

        public Condition? Condition
        {
            get => condition;
            set
            {
                if (value == condition) return;
                condition = value;
                OnPropertyChanged();
            }
        }

        public string? ConditionDetails
        {
            get => conditionDetails;
            set
            {
                if (value == conditionDetails) return;
                conditionDetails = value;
                OnPropertyChanged();
            }
        }

        public string? Notes
        {
            get => notes;
            set
            {
                if (value == notes) return;
                notes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ImageViewModel> Scans
        {
            get => scans;
            set
            {
                if (Equals(value, scans)) return;
                scans = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FileViewModel> Files
        {
            get => files;
            set
            {
                if (Equals(value, files)) return;
                files = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Condition> Conditions => Enum
            .GetValues(typeof(Condition))
            .Cast<Condition>()
            .ToList();

        public IEnumerable<ItemType> ItemTypes => Model.ItemTypes.All;

        public ICommand AddFileCommand => addFileCommand ?? (addFileCommand = new GameItemAddFileCommand(this));
        public ICommand RemoveFileCommand => removeFileCommand ?? (removeFileCommand = new GameItemRemoveFileCommand(this));
        public ICommand AddScanCommand => addScanCommand ?? (addScanCommand = new GameItemAddScanCommand(this));
        public ICommand RemoveScanCommand => removeScanCommand ?? (removeScanCommand = new GameItemRemoveScanCommand(this));

        public GameItem BuildItem()
        {
            return new GameItem
            {
                ItemType = ItemType,
                Missing = Missing,
                Condition = Condition,
                ConditionDetails = ConditionDetails,
                Notes = Notes,
                Files = Files.Select(vm => vm.BuildFile()).ToList(),
                Scans = Scans.Select(vm => vm.BuildImage()).ToList()
            };
        }

        public static ItemViewModel FromItem(GameItem gameItem) =>
            new ItemViewModel
            {
                ItemType = gameItem.ItemType,
                Missing = gameItem.Missing,
                Condition = gameItem.Condition,
                ConditionDetails = gameItem.ConditionDetails,
                Notes = gameItem.Notes,
                Scans = new ObservableCollection<ImageViewModel>(gameItem.Scans.Select(ImageViewModel.FromImage)),
                Files = new ObservableCollection<FileViewModel>(gameItem.Files.Select(FileViewModel.FromFile)),
            };
    }
}
