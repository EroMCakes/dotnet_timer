using System;

namespace TimerMicroservice.Models
{
    public class CreateTimerRequest
    {
        public Guid SessionId { get; set; }
        public TimeSpan TotalTime { get; set; }
    }
}