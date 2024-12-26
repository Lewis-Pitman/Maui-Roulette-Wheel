namespace Spinning_Wheel
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel mainPageViewModel;
        public MainPage()
        {
            InitializeComponent();
            mainPageViewModel = new MainPageViewModel(Wheel);
            BindingContext = mainPageViewModel;
        }
    }
}
