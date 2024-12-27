using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        private Page page;

        private WheelSpin wheelSpin;
        private bool isSpinning;
        private CancellationTokenSource cancellationTokenSource;

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
        public MainPageViewModel(GraphicsView _wheel, Page _page)
        {
            //Buttons
            SpinCommand = new Command(SpinWheel);
            AddItemCommand = new Command(AddItem);
            RemoveItemCommand = new Command<Item>(RemoveItem);

            //UI
            wheelDrawable = new WheelDrawable(Items);

            //Variables
            wheel = _wheel;
            page = _page;
            wheelSpin = new(wheel);
            cancellationTokenSource = new CancellationTokenSource();
        }
        #endregion

        #region Functions
        private async void SpinWheel()
        {
            if (Items.Count() > 0 && !isSpinning)
            {
                isSpinning = true;

                await wheelSpin.SpinAsync();

                int winnerIndex = (int)Math.Floor(wheel.Rotation) / ((int)360f / items.Count());
                //Integer division of the current rotation by the angle per sector will return the winning item's index in Items[]

                await Task.Delay(1000); //Delay one second

                bool removeItem = await page.DisplayAlert("Winner", Items[winnerIndex].Title, "Remove this item", "Close");

                if (removeItem)
                {
                    Items.RemoveAt(winnerIndex);
                    wheel.Invalidate();
                }

                wheel.Rotation = wheelSpin.currentAngle;
                isSpinning = false;
            }
            else if (Items.Count() <= 0)
            {
                await page.DisplayAlert("Alert", "Please add items to the wheel in order to spin it", "Close");
            }
        }

        private async void AddItem()
        {
            if (!string.IsNullOrWhiteSpace(EntryText))
            {
                Items.Add(new Item { Title = EntryText });
                EntryText = string.Empty;
                WheelDrawable = new WheelDrawable(Items);
            }
            else
            {
                await page.DisplayAlert("Alert", "An item cannot be empty. Please add at least 1 non-whitespace character", "Okay");
            }
        }

        private void RemoveItem(Item item)
        {
            Items.Remove(item);
            WheelDrawable = new WheelDrawable(Items);
        }
        #endregion
    }
}
