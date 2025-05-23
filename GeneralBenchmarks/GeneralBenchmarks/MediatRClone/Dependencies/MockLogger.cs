using System.Text;

namespace GeneralBenchmarks.MediatRClone.Dependencies;

public class MockLogger
{
    public string Log(string message) => new StringBuilder().AppendLine(message).ToString();
}