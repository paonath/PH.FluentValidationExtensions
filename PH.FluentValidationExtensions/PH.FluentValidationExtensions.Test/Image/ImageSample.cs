using System;

namespace PH.FluentValidationExtensions.Test.Image;

/// <summary>
/// 
/// </summary>
public class ImageSample
{
    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public ImageSample()
    {
        ContentType = String.Empty;
        FileName    = String.Empty;
    }
    /// <summary>
    /// 
    /// </summary>
    public string ContentType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string FileName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static ImageSample GoodInstance() => new ImageSample() { ContentType = "image/jpeg", FileName = "test.jpg" };
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static ImageSample BadInstance() => new ImageSample() { ContentType = "text/css", FileName = "error.css" };

}