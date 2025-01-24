using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
namespace NuoroLight.Application.Extensions;
public class ImageOptimizer
{
    public void ImageResizer(string inputImagePath, string outputImagePath, int? width, int? height)
    {
        var customWidth = width ?? 100;
        var customHeight = height ?? 100;

        // استفاده از FileStream برای خواندن فایل
        using (var inputStream = File.OpenRead(inputImagePath))
        {
            using (var image = Image.Load(inputStream))
            {
                image.Mutate(x => x.Resize(customWidth, customHeight));

                // ذخیره تصویر به صورت فایل
                using (var outputStream = File.OpenWrite(outputImagePath))
                {
                    image.Save(outputStream, new JpegEncoder
                    {
                        Quality = 100
                    });
                }
            }
        }
    }
}

