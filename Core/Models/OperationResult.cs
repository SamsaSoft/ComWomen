namespace Core.Models
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public T Data { get; }

        public OperationResult(bool isSuccess, string message, T data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }
    }
}
