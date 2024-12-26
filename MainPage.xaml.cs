namespace Spinning_Wheel
{
    public partial class MainPage : ContentPage
    {
        private SectorDrawable sector;
        private MainPageViewModel mainPageViewModel = new();
        public MainPage()
        {
            InitializeComponent();
            sector = new SectorDrawable();
            Wheel.Drawable = sector;
            BindingContext = mainPageViewModel;
        }
    }
}
