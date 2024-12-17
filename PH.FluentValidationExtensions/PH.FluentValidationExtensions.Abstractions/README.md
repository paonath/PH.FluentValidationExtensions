# PH.FluentValidationExtensions.Abstractions

Some abstractions and attribute.

## DisableScriptCheckValidationAttribute

Disable Check in Rule `.WithNoScripts()` with a simple attribute
```csharp

 public class DemoClassToValidate 
 {
    /// <summary>
    /// This string value skip the .WithNoScripts() validation
    /// </summary>
     [DisableScriptCheckValidation]    
     public string StringValueSkipValidation { get; set; }
    
    /// <summary>
    /// This string value perform validation
    /// </summary>
	public string StringValue { get; set; }
	
     
 }

```