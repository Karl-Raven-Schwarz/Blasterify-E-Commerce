namespace Blasterify.Models
{
    public class Result<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }


        public Result(bool success)
        {
            Success = success;
        }

        public Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public Result(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public Result()
        {
        }
    }
}