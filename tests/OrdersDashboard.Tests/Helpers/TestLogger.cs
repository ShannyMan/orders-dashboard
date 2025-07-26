using Microsoft.Extensions.Logging;

namespace OrdersDashboard.Tests.Helpers;

public class TestLogEntry
{
    public LogLevel Level { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public IReadOnlyList<KeyValuePair<string, object?>> StructuredState { get; set; } = new List<KeyValuePair<string, object?>>();
    public Exception? Exception { get; set; }
}

public class TestLogger<T> : ILogger<T>
{
    private readonly List<TestLogEntry> _logEntries = new();
    private readonly string _categoryName;

    public TestLogger()
    {
        _categoryName = typeof(T).FullName ?? typeof(T).Name;
    }

    public IReadOnlyList<TestLogEntry> LogEntries => _logEntries;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var structuredState = new List<KeyValuePair<string, object?>>();
        
        if (state is IReadOnlyList<KeyValuePair<string, object?>> stateList)
        {
            structuredState.AddRange(stateList);
        }

        _logEntries.Add(new TestLogEntry
        {
            Level = logLevel,
            Category = _categoryName,
            Message = formatter(state, exception),
            StructuredState = structuredState,
            Exception = exception
        });
    }

    public void Clear()
    {
        _logEntries.Clear();
    }
}