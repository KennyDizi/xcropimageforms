using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ReactiveUI;
using Xamarin.Forms;
using XCrossCropImage.SourceCode.DPServices;

namespace XCrossCropImage.SourceCode
{
    public class MainPageViewModel : ReactiveObject
    {
        public MainPageViewModel()
        {
            TakePhotoCommand = new Command(TakePhotoCommandAction);
            ChoosePhotoCommand = new Command(ChoosePhotoCommandAction);
        }

        /// <summary>
        /// AvatarImageSource - image source cua nguoi hien tai
        /// </summary>
        private ImageSource _avatarImageSource;

        public ImageSource AvatarImageSource
        {
            get { return _avatarImageSource; }
            set { this.RaiseAndSetIfChanged(ref _avatarImageSource, value); }
        }

        public ICommand TakePhotoCommand { get; }

        private async void TakePhotoCommandAction()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                Debug.WriteLine("No Camera avaialble");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                SaveToAlbum = true,
                DefaultCamera = CameraDevice.Front,
                Directory = "VaccineProfileFolder",
                Name = "YourAvata.jpg"
            });

            if (file == null)
                return;
            var cropedBytes = await CrossXMethod.Current.CropImageFromOriginalToBytes(file.Path);

            if (cropedBytes != null)
                AvatarImageSource = ImageSource.FromStream(() =>
                {
                    var cropedImage = new MemoryStream(cropedBytes);
                    file.Dispose();
                    return cropedImage;
                });
            else
            {
                file.Dispose();
            }
        }

        public ICommand ChoosePhotoCommand { get; }

        private async void ChoosePhotoCommandAction()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Debug.WriteLine("No Pick Photo Supported");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            var cropedBytes = await CrossXMethod.Current.CropImageFromOriginalToBytes(file.Path);

            if (cropedBytes != null)
                AvatarImageSource = ImageSource.FromStream(() =>
                {
                    var cropedImage = new MemoryStream(cropedBytes);
                    file.Dispose();
                    return cropedImage;
                });
            else
            {
                file.Dispose();
            }
        }
    }
}
