using Application.Models.Mapping;
using ApplicationData.Contract.Models.DataModels;
using AutoMapper;

namespace Application.Models.Dto
{
    public class PasswordDto : IMapFrom<PasswordDataModel>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? Salt { get; set; }
        public string? Hash { get; set; }
        public bool? IsActive { get; set; }

        public void Mapping(
            Profile profile)
        {
            profile.CreateMap<PasswordDataModel, PasswordDto>();
        }
    }
}
