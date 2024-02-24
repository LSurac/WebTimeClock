using AutoMapper;
using WebTimeClock.Application.Models.Mapping;
using WebTimeClock.ApplicationData.Contract.Models.DataModels;

namespace WebTimeClock.Application.Models.Dto
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
