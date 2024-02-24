using Microsoft.AspNetCore.Mvc;
using WebTimeClock.Application.TimeClock.Commands.TimeClockSet;
using WebTimeClock.Application.TimeClock.Queries.TimeClockLastGet;
using WebTimeClock.Application.TimeClock.Queries.TimeClockListGet;

namespace WebTimeClock.WebUi.Controllers
{
    //[Authorize]
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
