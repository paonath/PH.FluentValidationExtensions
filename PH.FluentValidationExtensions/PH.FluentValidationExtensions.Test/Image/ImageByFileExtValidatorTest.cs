using System;
using System.Threading.Tasks;
using FluentValidation;
using Xunit;

namespace PH.FluentValidationExtensions.Test.Image;

public class ImageByFileExtValidatorTest
{
    [Fact]
    public void Test()
    {
        var validator = new ImageByFileExtValidatorToBeTested();

        var good = ImageSample.GoodInstance();
        var bad = ImageSample.BadInstance();

        var isGood = validator.Validate(good);
        var isBad = validator.Validate(bad);

        Assert.True(isGood.IsValid);

        Assert.False(isBad.IsValid);
    }

    [Fact]
    public void TestWithListOfExtensions()
    {
        var validator = new ExtendedImageByFileExtValidatorToBeTested(); //this allow only png and jpg
        var good = ImageSample.GoodInstance();
        var bad = new ImageSample() { FileName = "notAllowed.gif" };

        var isGood = validator.Validate(good);
        var isBad = validator.Validate(bad);



        Assert.True(isGood.IsValid);

        Assert.False(isBad.IsValid);

    }


    [Fact]
    public async Task TestAsync()
    {
        var validator = new ImageByFileExtValidatorToBeTested();

        var good = ImageSample.GoodInstance();

        var isGood = await validator.ValidateAsync(good);

        Assert.True(isGood.IsValid);
    }

    [Fact]
    public void TestException()
    {
        Exception? e = null;
        try
        {
            var a = new InvalidImageByFileExtValidatorToBeTested()
            {
                ClassLevelCascadeMode = CascadeMode.Stop
            };
            Assert.Equal(CascadeMode.Continue, a.ClassLevelCascadeMode);
        }
        catch (Exception? exception)
        {
            e = exception;
        }

        Assert.NotNull(e);
    }
}