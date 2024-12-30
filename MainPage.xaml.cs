namespace Spinning_Wheel
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel mainPageViewModel;
        public MainPage()
        {
            InitializeComponent();
            mainPageViewModel = new MainPageViewModel(Wheel, this);
            BindingContext = mainPageViewModel;

            if (OperatingSystem.IsWindows())
            {
                DisplayAlert("Alert", "Clip path is not supported on Windows, so the wheel will display incorrectly. Please use android for the best experience.", "Okay");
            }
        }
    }
}
