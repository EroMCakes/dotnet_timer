using System;

namespace TimerMicroservice.Models
{
    public class UpdateTotalTimeRequest
    {
        public TimeSpan NewTotalTime { get; set; }
    }
}