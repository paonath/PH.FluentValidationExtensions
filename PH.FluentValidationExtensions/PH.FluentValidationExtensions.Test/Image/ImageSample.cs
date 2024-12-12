namespace PH.FluentValidationExtensions.Test.Image;

public class ImageSample
{
    public string ContentType { get; set; }
    public string FileName { get; set; }

    public static ImageSample GoodInstance() => new ImageSample() { ContentType = "image/jpeg", FileName = "test.jpg" };

    public static ImageSample BadInstance() => new ImageSample() { ContentType = "text/css", FileName = "error.css" };

}