using System.IO;

namespace PH.FluentValidationExtensions.Test;

public class ImageSample
{
	public string   ContentType { get; set; }
	public string   FileName    { get; set; }
	public FileInfo FileInfo    => new FileInfo(FileName);

	public static ImageSample GoodInstance() => new ImageSample() { ContentType = "image/jpeg" , FileName = "test.jpg"};

	public static ImageSample BadInstance() => new ImageSample() { ContentType = "text/css", FileName = "error.css" };

}