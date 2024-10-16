using System;

namespace TimerMicroservice.Models
{
    public class Timer
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public TimeSpan TotalTime { get; set; }
        public bool IsRunning { get; set; }
        public DateTime? StartedAt { get; set; }
        public TimeSpan ElapsedTime { get; set; }
    }
}