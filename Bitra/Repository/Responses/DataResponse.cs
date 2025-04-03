

namespace Repository.Responses
{
    public class DataResponse<T> : ApiResponse
    {
        public T? Data { get; set; }

        public DataResponse(int statusCode, string title, string message, T? data) : base(statusCode, title, message)
        {
            Data = data;
        }
    }
}
