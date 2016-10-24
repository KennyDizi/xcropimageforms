using XCrossCropImage.SourceCode;

namespace XCrossCropImage
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }
    }
}
