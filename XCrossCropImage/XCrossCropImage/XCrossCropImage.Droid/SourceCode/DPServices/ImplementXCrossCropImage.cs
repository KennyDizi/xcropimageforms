using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using XCrossCropImage.Droid.SourceCode.DPServices;
using XCrossCropImage.Droid.SourceCode.XCropImage;
using XCrossCropImage.SourceCode.DPServices;
using XFMiniMobileApplication.SourceCode.Utilites.Events;

[assembly: Dependency(typeof(ImplementXCrossCropImage))]

namespace XCrossCropImage.Droid.SourceCode.DPServices
{
    public class ImplementXCrossCropImage : IXCrossCropImage
    {
        #region cropimage

        private int GetRequestId()
        {
            var id = _requestId;
            if (_requestId == int.MaxValue)
                _requestId = 0;
            else
                _requestId++;

            return id;
        }

        private int _requestId;
        private TaskCompletionSource<byte[]> _completionSource;

        public Task<byte[]> CropImageFromOriginalToBytes(string filePath)
        {
            var id = GetRequestId();

            var ntcs = new TaskCompletionSource<byte[]>(id);
            if (Interlocked.CompareExchange(ref _completionSource, ntcs, null) != null)
            {
#if DEBUG
                throw new InvalidOperationException("Only one operation can be active at a time");
#else
                return null;
#endif
            }

            var intent = new Intent(CrossCurrentActivity.Current.Activity, typeof(CropImage));
            intent.PutExtra("image-path", filePath);
            intent.PutExtra("scale", true);

            //event
            EventHandler<XViewEventArgs> handler = null;
            handler = (s, e) =>
            {
                var tcs = Interlocked.Exchange(ref _completionSource, null);

                CropImage.MediaCroped -= handler;
                tcs.SetResult((e.CastObject as Bitmap)?.BitmapToBytes());
            };

            CropImage.MediaCroped += handler;
            CrossCurrentActivity.Current.Activity.StartActivity(intent);

            return _completionSource.Task;
        }
        #endregion
    }
}