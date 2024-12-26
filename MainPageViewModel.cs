using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Spinning_Wheel
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        #region Interface implementation
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Variables
        private ObservableCollection<Item> items;
        public ObservableCollection<Item> Items
        {
            get => items;
            set
            {
                if (items != value)
                {
                    items = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }

        private string entryText;
        public string EntryText
        {
            get => entryText;
            set
            {
                entryText = value;
                OnPropertyChanged(nameof(EntryText));
            }

        }

        public ICommand SpinCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand RemoveItemCommand { get; }
        #endregion

        #region Constructor
        public MainPageViewModel()
        {
            SpinCommand = new Command(SpinWheel);
            AddItemCommand = new Command(AddItem);
            RemoveItemCommand = new Command<Item>(RemoveItem);
            Items = new ObservableCollection<Item>();
        }
        #endregion

        #region Functions
        private void SpinWheel()
        {
            // Spin logic
        }

        private void AddItem()
        {
            Items.Add(new Item { Title = EntryText });
            EntryText = string.Empty;
        }

        private void RemoveItem(Item item)
        {
            Items.Remove(item);
        }
        #endregion
    }
}
