using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimerMicroservice.Models;

namespace TimerMicroservice.Services
{
    public interface ITimerService
    {
        Task<Models.Timer> CreateTimer(Guid sessionId, TimeSpan totalTime);
        Task StartTimer(Guid timerId);
        Task PauseTimer(Guid timerId);
        Task StopTimer(Guid timerId);
        Task UpdateTotalTime(Guid timerId, TimeSpan newTotalTime);
        Models.Timer GetTimer(Guid timerId);
        IEnumerable<Models.Timer> GetAllTimers();
    }
}