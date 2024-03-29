﻿using AutoMapper;
using MediatR;
using WebTimeClock.Application.Models.Dto;
using WebTimeClock.ApplicationData.Contract.Services;

namespace WebTimeClock.Application.TimeClock.Queries.TimeClockLastGet
{
    public class TimeClockLastGetQueryHandler(IEmployeeDataService employeeDataService, 
        ITimeClockDataService timeClockDataService,
        IMapper mapper) : IRequestHandler<TimeClockLastGetQuery, TimeClockLastGetQueryResult>
    {
        public async Task<TimeClockLastGetQueryResult> Handle(
            TimeClockLastGetQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.EmployeeId))
            {
                throw new Exception("EmployeeId is missing");
            }

            _ = employeeDataService.GetEmployeeAsync(request.EmployeeId) 
                ?? throw new Exception($"Invalid EmployeeId {request.EmployeeId}");

            var timeClockDataModel = timeClockDataService.GetLastTimeClockDataAsync(request.EmployeeId);

            return new TimeClockLastGetQueryResult
            {
                TimeClock = mapper.Map<TimeClockDto>(timeClockDataModel)
            };
        }
    }
}
