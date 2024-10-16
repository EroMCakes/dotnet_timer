using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimerMicroservice.Models;
using TimerMicroservice.Services;

namespace TimerMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "TeamMember")]
    public class TimerController : ControllerBase
    {
        private readonly ITimerService _timerService;
        private readonly ILogger<TimerController> _logger;

        public TimerController(ITimerService timerService, ILogger<TimerController> logger)
        {
            _timerService = timerService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Models.Timer>> CreateTimer(CreateTimerRequest request)
        {
            var timer = await _timerService.CreateTimer(request.SessionId, request.TotalTime);
            return CreatedAtAction(nameof(GetTimer), new { id = timer.Id }, timer);
        }

        [HttpGet("{id}")]
        public ActionResult<Models.Timer> GetTimer(Guid id)
        {
            var timer = _timerService.GetTimer(id);
            if (timer == null)
                return NotFound();
            return timer;
        }

        [HttpPost("{id}/start")]
        public async Task<IActionResult> StartTimer(Guid id)
        {
            await _timerService.StartTimer(id);
            return NoContent();
        }

        [HttpPost("{id}/pause")]
        public async Task<IActionResult> PauseTimer(Guid id)
        {
            await _timerService.PauseTimer(id);
            return NoContent();
        }

        [HttpPost("{id}/stop")]
        public async Task<IActionResult> StopTimer(Guid id)
        {
            await _timerService.StopTimer(id);
            return NoContent();
        }

        [HttpPut("{id}/totaltime")]
        public async Task<IActionResult> UpdateTotalTime(Guid id, UpdateTotalTimeRequest request)
        {
            await _timerService.UpdateTotalTime(id, request.NewTotalTime);
            return NoContent();
        }

        [Authorize(Policy = "GM")]
        [HttpGet("all")]
        public ActionResult<IEnumerable<Models.Timer>> GetAllTimers()
        {
            return Ok(_timerService.GetAllTimers());
        }
    }
}