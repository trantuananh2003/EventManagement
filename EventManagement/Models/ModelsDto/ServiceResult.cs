namespace EventManagement.Models.ModelsDto
{
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public List<string>? Message { get; set; }

        public ServiceResult()
        {
            Message = new List<string>();
        }
    }
}
