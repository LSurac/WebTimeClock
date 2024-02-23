using Application.TimeClock.Commands.TimeClockSet;
using Application.TimeClock.Queries.TimeClockLastGet;
using Application.TimeClock.Queries.TimeClockListGet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebTimeClock.Controllers
{
    [Authorize]
    [ApiController]
    public class TimeClockController : BaseController<TimeClockController>
    {
        [HttpPost]
        [ProducesResponseType(typeof(TimeClockLastGetQueryResult), StatusCodes.Status200OK)]
        public async Task<ActionResult> TimeClockLastGet(TimeClockLastGetQuery timeClockLastGetQuery)
        {
            return Ok(await Mediator.Send(timeClockLastGetQuery));
        }

        [HttpPost]
        [ProducesResponseType(typeof(TimeClockListGetQueryResult), StatusCodes.Status200OK)]
        public async Task<ActionResult> TimeClockListGet(TimeClockListGetQuery timeClockListGetQueryResult)
        {
            return Ok(await Mediator.Send(timeClockListGetQueryResult));
        }

        [HttpPost]
        [ProducesResponseType(typeof(TimeClockSetCommandResult), StatusCodes.Status200OK)]
        public async Task<ActionResult> TimeClockSet(TimeClockSetCommand timeClockSetCommand)
        {
            return Ok(await Mediator.Send(timeClockSetCommand));
        }
    }
}
