using Application.Models.Mapping;
using ApplicationData.Contract.Models.DataModels;
using ApplicationData.Contract.Models.Enums;
using AutoMapper;

namespace Application.Models.Dto
{
    public class EmployeeDto : IMapFrom<EmployeeDataModel>
    {
        public int Id { get; set; }
        public string? ForName { get; set; }
        public string? LastName { get; set; }
        public EGender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int AddressId { get; set; } //TODO: Das hier irgendwann mit einem AddressDto ersetzen
        public DateOnly HireDate { get; set; }
        public int DepartmentId { get; set; } //TODO: Das hier irgendwann mit einem DepartmentDto ersetzen

        public void Mapping(
            Profile profile)
        {
            profile.CreateMap<EmployeeDataModel, EmployeeDto>();
        }
    }
}
