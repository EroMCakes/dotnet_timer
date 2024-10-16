using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimerMicroservice.Hubs;
using TimerMicroservice.Models;

namespace TimerMicroservice.Services
{
    public class TimerService : ITimerService
    {
        private readonly Dictionary<Guid, Models.Timer> _timers = new();
        private readonly IHubContext<TimerHub> _hubContext;
        private readonly ILogger<TimerService> _logger;

        public TimerService(IHubContext<TimerHub> hubContext, ILogger<TimerService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<Models.Timer> CreateTimer(Guid sessionId, TimeSpan totalTime)
        {
            var timer = new Models.Timer
            {
                Id = Guid.NewGuid(),
                SessionId = sessionId,
                TotalTime = totalTime,
                IsRunning = false,
                ElapsedTime = TimeSpan.Zero
            };

            _timers[timer.Id] = timer;
            await NotifyTimerUpdate(timer);
            return timer;
        }

        public async Task StartTimer(Guid timerId)
        {
            if (_timers.TryGetValue(timerId, out var timer))
            {
                timer.IsRunning = true;
                timer.StartedAt = DateTime.UtcNow;
                await NotifyTimerUpdate(timer);
            }
        }

        public async Task PauseTimer(Guid timerId)
        {
            if (_timers.TryGetValue(timerId, out var timer))
            {
                timer.IsRunning = false;
                timer.ElapsedTime += DateTime.UtcNow - timer.StartedAt.Value;
                timer.StartedAt = null;
                await NotifyTimerUpdate(timer);
            }
        }

        public async Task StopTimer(Guid timerId)
        {
            if (_timers.TryGetValue(timerId, out var timer))
            {
                timer.IsRunning = false;
                timer.ElapsedTime = TimeSpan.Zero;
                timer.StartedAt = null;
                await NotifyTimerUpdate(timer);
            }
        }

        public async Task UpdateTotalTime(Guid timerId, TimeSpan newTotalTime)
        {
            if (_timers.TryGetValue(timerId, out var timer))
            {
                timer.TotalTime = newTotalTime;
                await NotifyTimerUpdate(timer);
            }
        }

        public Models.Timer GetTimer(Guid timerId)
        {
            return _timers.TryGetValue(timerId, out var timer) ? timer : null;
        }

        public IEnumerable<Models.Timer> GetAllTimers()
        {
            return _timers.Values;
        }

        private async Task NotifyTimerUpdate(Models.Timer timer)
        {
            await _hubContext.Clients.Group(timer.SessionId.ToString()).SendAsync("TimerUpdated", timer);
        }
    }
}
