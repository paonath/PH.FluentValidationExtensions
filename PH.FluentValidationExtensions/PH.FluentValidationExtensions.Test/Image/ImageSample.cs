using System;

namespace PH.FluentValidationExtensions.Test.Image;

public class ImageSample
{
    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public ImageSample()
    {
        ContentType = String.Empty;
        FileName    = String.Empty;
    }

    public string ContentType { get; set; }
    public string FileName { get; set; }

    public static ImageSample GoodInstance() => new ImageSample() { ContentType = "image/jpeg", FileName = "test.jpg" };

    public static ImageSample BadInstance() => new ImageSample() { ContentType = "text/css", FileName = "error.css" };

}