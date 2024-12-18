#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace PH.FluentValidationExtensions.Validators.Image
{
    /// <summary>
    ///     A List of image content types
    /// </summary>
    internal static class ImageContentTypesDictionary
    {
        public static readonly string[] All =
        {
            "image/bmp", "image/cis-cod", "image/gif", "image/ief", "image/jpeg", "image/pipeg", "image/png",
            "image/svg+xml", "image/tiff", "image/tiff", "image/webp", "image/x-cmu-raster", "image/x-cmx",
            "image/x-icon", "image/x-portable-anymap", "image/x-portable-bitmap", "image/x-portable-graymap",
            "image/x-portable-pixmap", "image/x-rgb", "image/x-xbitmap", "image/x-xpixmap", "image/x-xwindowdump"
        };
    }
}