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
        private GraphicsView wheel;

        private ObservableCollection<Item> items = new ObservableCollection<Item>();
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

        private WheelDrawable wheelDrawable;
        public WheelDrawable WheelDrawable
        {
            get => wheelDrawable;
            set
            {
                wheelDrawable = value;
                OnPropertyChanged(nameof(WheelDrawable));
                wheel.Invalidate();
            }

        }

        private string resultText = "Spin the wheel!";
        public string ResultText
        {
            get => resultText;
            set
            {
                resultText = value;
                OnPropertyChanged(nameof(ResultText));
            }

        }

        private string entryText = string.Empty;
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
        public MainPageViewModel(GraphicsView _wheel)
        {
            //Buttons
            SpinCommand = new Command(SpinWheel);
            AddItemCommand = new Command(AddItem);
            RemoveItemCommand = new Command<Item>(RemoveItem);

            //UI
            wheelDrawable = new WheelDrawable(Items);

            //Variables
            wheel = _wheel;
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
            WheelDrawable = new WheelDrawable(Items);
        }

        private void RemoveItem(Item item)
        {
            Items.Remove(item);
            WheelDrawable = new WheelDrawable(Items);
        }
        #endregion
    }
}
