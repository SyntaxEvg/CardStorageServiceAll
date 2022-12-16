namespace CardStorageServiceAll.Interfaces
{
    public interface IOperationResult
    {
        int ErrorCode { get; }

        string? ErrorMessage { get; }
    }
}
