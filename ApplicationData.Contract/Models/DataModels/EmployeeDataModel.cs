using WebTimeClock.ApplicationData.Contract.Models.Enums;

namespace WebTimeClock.ApplicationData.Contract.Models.DataModels
{
    public class EmployeeDataModel
    {
        public int Id { get; set; }
        public string? ForName { get; set; }
        public string? LastName { get; set; }
        public EGender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int AddressId { get; set; } //TODO: Das hier irgendwann mit einem AddressDataModel ersetzen
        public DateOnly HireDate { get; set; }
        public int DepartmentId { get; set; } //TODO: Das hier irgendwann mit einem DepartmentDataModel ersetzen
    }
}
