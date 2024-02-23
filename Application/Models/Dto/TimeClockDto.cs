using Application.Models.Mapping;
using ApplicationData.Contract.Models.DataModels;
using ApplicationData.Contract.Models.Enums;
using AutoMapper;

namespace Application.Models.Dto
{
    public class TimeClockDto : IMapFrom<TimeClockDataModel>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime UtcTime { get; set; }
        public ETimeClockAction Action { get; set; }

        public void Mapping(
            Profile profile)
        {
            profile.CreateMap<TimeClockDataModel, TimeClockDto>();
        }
    }
}
