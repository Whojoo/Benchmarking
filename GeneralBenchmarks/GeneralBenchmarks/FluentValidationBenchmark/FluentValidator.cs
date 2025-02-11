using FluentValidation;

namespace GeneralBenchmarks.FluentValidationBenchmark;

public class FluentValidator : AbstractValidator<Model>
{
    public FluentValidator()
    {
        RuleFor(m => m.FullName)
            .NotEmpty();

        RuleFor(m => m.DateOfBirth)
            .Must(date => date < DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
            .WithMessage("Person must be at least 18 years old");

        RuleFor(m => m.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(m => m.PhoneNumber)
            .NotEmpty()
            .Matches(RegexCollection.PhoneRegex())
            .WithMessage("PhoneNumber should be a valid 10 digit phone number");

        RuleFor(m => m.Gender)
            .IsInEnum();
    }
}