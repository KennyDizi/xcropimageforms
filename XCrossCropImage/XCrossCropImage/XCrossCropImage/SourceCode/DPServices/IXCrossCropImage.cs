using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XCrossCropImage.SourceCode.DPServices
{
    public interface IXCrossCropImage
    {
        Task<byte[]> CropImageFromOriginalToBytes(string filePath);
    }

    public class CrossXMethod
    {
        private static readonly Lazy<IXCrossCropImage> Implementation = new Lazy<IXCrossCropImage>(CreateMedia,
            System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IXCrossCropImage Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
#if DEBUG
                    throw NotImplementedInReferenceAssembly();
#endif
                }
                return ret;
            }
        }

        private static IXCrossCropImage CreateMedia()
        {
#if PORTABLE
            return null;
#else
            return DependencyService.Get<IXCrossCropImage>();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
