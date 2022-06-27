namespace StoredProcedureApi.Models
{
    public class ResponseModel
    {
        public int ErrorStatus { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Result { get; set; }
    }
}