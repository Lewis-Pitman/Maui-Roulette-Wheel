namespace Spinning_Wheel
{
    public partial class MainPage : ContentPage
    {
        private SectorDrawable sector;
        public MainPage()
        {
            InitializeComponent();
            sector = new SectorDrawable();
            Wheel.Drawable = sector;
        }

        public void RandomClicked(object sender, EventArgs e)
        {
            Random random = new Random();
            sector.changeNumberOfSectors(random.Next(50));
            Wheel.Invalidate();
        }

        public void RotateClicked(object sender, EventArgs e)
        {
            Random random = new Random();
            Wheel.Rotation = random.Next(360);
        }
    }

}
