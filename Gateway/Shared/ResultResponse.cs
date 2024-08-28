namespace Gateway.Shared
{
    public class ResultResponse<T>
    {
        public T? Value { get; init; }
        public ErrorType? ErrorType { get; init; }
        public string? ErrorMessage { get; init; }
    }

    public enum ErrorType
    {
        NotFound,
        Validation,
        DatabaseException,
        DuplicatedEntry
    }
}
