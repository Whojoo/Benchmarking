using System.Text.RegularExpressions;

namespace GeneralBenchmarks.FluentValidationBenchmark;

public static partial class RegexCollection
{
    [GeneratedRegex(@"^(\+\d{1,2}\s?)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$")]
    public static partial Regex PhoneRegex();
}