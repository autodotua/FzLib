namespace FzLib.Avalonia.Dialogs;

public class ValidationResult
{
    private ValidationResult()
    {
    }

    public static ValidationResult Valid()
    {
        return new ValidationResult { IsValid = true };
    }

    public static ValidationResult Error(string message)
    {
        return new ValidationResult
        {
            IsValid = false,
            ErrorMessage = message
        };
    }

    public bool IsValid { get; private init; }
    public string ErrorMessage { get; private init; }
}