using AutoMapper;
using WebTimeClock.Application.Models.Mapping;
using WebTimeClock.ApplicationData.Contract.Models.DataModels;
using WebTimeClock.ApplicationData.Contract.Models.Enums;

namespace WebTimeClock.Application.Models.Dto
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
