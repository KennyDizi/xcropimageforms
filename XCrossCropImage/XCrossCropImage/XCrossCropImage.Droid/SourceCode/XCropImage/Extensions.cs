using System.IO;
using Android.Graphics;

namespace XCrossCropImage.Droid.SourceCode.XCropImage
{
    public static class Extensions
    {
        public static byte[] BitmapToBytes(this Bitmap myBitmapImage)
        {
            var ms = new MemoryStream();
            // Converting Bitmap image to byte[] array
            myBitmapImage.Compress(Bitmap.CompressFormat.Png, 0, ms);
            var imageByteArray = ms.ToArray();
            return imageByteArray;
        }
    }
}