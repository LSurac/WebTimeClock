namespace ApplicationData.Contract.Models.DataModels
{
    public class PasswordDataModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? Salt { get; set; }
        public string? Hash { get; set; }
        public bool? IsActive { get; set; }
    }
}
