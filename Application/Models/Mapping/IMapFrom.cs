using AutoMapper;

namespace WebTimeClock.Application.Models.Mapping
{
    public interface IMapFrom<T>
    {
        void Mapping(
            Profile profile)
        {
            profile.CreateMap(typeof(T), GetType());
        }
    }
}
