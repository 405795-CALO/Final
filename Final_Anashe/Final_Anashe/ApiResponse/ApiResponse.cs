using System.Net;

namespace Final_Anashe.ApiResponse
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ApiResponse()
        {
            Success = true;
            ErrorMessage = "";
            StatusCode = HttpStatusCode.OK;
        }

        public void SetError(string mensaje, HttpStatusCode statusCode)
        {
            Success = false;
            ErrorMessage = mensaje;
            StatusCode = statusCode;
        }
    }
}
