

namespace Repository.Responses
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Title { get; set; }
        public string Messsage { get; set; }

        public ApiResponse(int statusCode = 200, string title = "Success", string message = "Proccess is successed.")
        {
            StatusCode = statusCode;
            Title = title;
            Messsage = message;
        }
    }
}
