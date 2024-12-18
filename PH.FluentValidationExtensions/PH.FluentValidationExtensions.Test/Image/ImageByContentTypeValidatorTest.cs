using System;
using System.Threading.Tasks;
using FluentValidation;
using Xunit;

namespace PH.FluentValidationExtensions.Test.Image;

/// <summary>
/// 
/// </summary>
public class ImageByContentTypeValidatorTest
{
    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void Test()
    {
        var validator = new ImageByContentTypeValidatorToBeTested();

        var good = ImageSample.GoodInstance();
        var bad = ImageSample.BadInstance();

        var isGood = validator.Validate(good);
        var isBad = validator.Validate(bad);



        Assert.True(isGood.IsValid);

        Assert.False(isBad.IsValid);
    }
    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void TestWithListOfContentTypes()
    {
        var validator = new ExtendedImageByContentTypeValidatorToBeTested(); //this allow only png and jpg
        var good = ImageSample.GoodInstance();
        var bad = new ImageSample() { ContentType = "image/bmp" };

        var isGood = validator.Validate(good);
        var isBad = validator.Validate(bad);



        Assert.True(isGood.IsValid);

        Assert.False(isBad.IsValid);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestAsync()
    {
        var validator = new ImageByContentTypeValidatorToBeTested();

        var good = ImageSample.GoodInstance();

        var isGood = await validator.ValidateAsync(good);

        Assert.True(isGood.IsValid);
    }
    
    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void TestException()
    {
        #if NET6_0
        Exception e = null;
        #else
        Exception? e = null;
        #endif
        
        try
        {
            var a = new InvalidImageByContentTypeValidatorToBeTested
            {
                ClassLevelCascadeMode = CascadeMode.Continue
            };
            
            Assert.Equal(CascadeMode.Stop,a.ClassLevelCascadeMode);
        }
        catch (Exception exception)
        {
            e = exception;
        }

        Assert.NotNull(e);
    }
}
