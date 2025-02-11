using FluentValidation.Results;

namespace GeneralBenchmarks.FluentValidationBenchmark;

public interface ICustomValidator<in T>
{
    ValidationResult Validate(T model);
}

public class CustomValidator : ICustomValidator<Model>
{
    private const string PropertyFullName = nameof(Model.FullName);
    private const string PropertyDateOfBirth = nameof(Model.DateOfBirth);
    private const string PropertyEmail = nameof(Model.Email);
    private const string PropertyPhoneNumber = nameof(Model.PhoneNumber);
    private const string PropertyGender = nameof(Model.Gender);

    private const string NotEmptyFormat = "{0} cannot be empty";
    
    public ValidationResult Validate(Model model)
    {
        List<ValidationFailure> errors = [];

        errors = ValidateFullName(model.FullName, errors);
        errors = ValidateDateOfBirth(model.DateOfBirth, errors);
        errors = ValidateEmail(model.Email, errors);
        errors = ValidatePhoneNumber(model.PhoneNumber, errors);
        errors = ValidateGender(model.Gender, errors);

        return new ValidationResult(errors);
    }

    private static List<ValidationFailure> ValidateFullName(string? fullName, List<ValidationFailure> errors)
    {
        if (string.IsNullOrEmpty(fullName))
        {
            errors.Add(new ValidationFailure(
                PropertyFullName,
                string.Format(NotEmptyFormat, PropertyFullName)));
        }

        return errors;
    }

    private static List<ValidationFailure> ValidateDateOfBirth(
        DateOnly dateOfBirth,
        List<ValidationFailure> errors)
    {
        const int minimumAgeInYears = 18;
        var targetDateTime = DateTime.Today.AddYears(-minimumAgeInYears);

        if (DateOnly.FromDateTime(targetDateTime) < dateOfBirth)
        {
            errors.Add(new ValidationFailure(
                PropertyDateOfBirth,
                "Person must be at least 18 years old"));
        }

        return errors;
    }

    private static List<ValidationFailure> ValidateEmail(string? email, List<ValidationFailure> errors)
    {
        var isEmpty = string.IsNullOrEmpty(email);

        switch (isEmpty)
        {
            case true:
                errors.Add(new ValidationFailure(
                    PropertyEmail,
                    string.Format(NotEmptyFormat, PropertyEmail)));
                break;
            case false:
            {
                var index = email!.IndexOf('@');
                var isFirstLetter = index == 0;
                var isLastLetter = index == email.Length - 1;
                var isOnlyCharacter = email.LastIndexOf('@') == index;

                if (isFirstLetter || isLastLetter || !isOnlyCharacter)
                {
                    errors.Add(new ValidationFailure(
                        PropertyEmail,
                        $"{PropertyEmail} is not a valid email"));
                }

                break;
            }
        }

        return errors;
    }

    private static List<ValidationFailure> ValidatePhoneNumber(
        string? phoneNumber,
        List<ValidationFailure> errors)
    {
        var isEmpty = string.IsNullOrEmpty(phoneNumber);

        switch (isEmpty)
        {
            case true:
                errors.Add(new ValidationFailure(
                    PropertyPhoneNumber,
                    string.Format(NotEmptyFormat, PropertyPhoneNumber)));
                break;
            case false:
                if (!RegexCollection.PhoneRegex().IsMatch(phoneNumber!))
                {
                    errors.Add(new ValidationFailure(
                        PropertyPhoneNumber,
                        "PhoneNumber should be a valid 10 digit phone number"));
                }
                break;
        }

        return errors;
    }

    private static List<ValidationFailure> ValidateGender(Gender gender, List<ValidationFailure> errors)
    {
        if (gender is > Gender.Other or < Gender.Male)
        {
            errors.Add(new ValidationFailure(
                PropertyGender,
                $"{gender} is not a valid value for Gender"));
        }

        return errors;
    }
}
