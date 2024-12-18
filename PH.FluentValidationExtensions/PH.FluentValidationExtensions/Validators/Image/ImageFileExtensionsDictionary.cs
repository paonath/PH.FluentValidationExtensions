#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace PH.FluentValidationExtensions.Validators.Image
{
    /// <summary>
    ///     A List of image file extensions
    /// </summary>
    internal static class ImageFileExtensionsDictionary
    {
        public static readonly string[] All =
        {
            "bmp", "cod", "gif", "ief", "jpe", "jpeg", "jpg", "jfif", "svg", "tif", "tiff", "ras", "cmx", "ico", "pnm",
            "pbm", "pgm", "png", "ppm", "rgb", "webp", "xbm", "xpm", "xwd"
        };
    }
}