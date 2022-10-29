using System;

namespace hu_app.Models
{
    public class Logs
    {
        public int Id { get; set; }
        public DateTime LogTime { get; set; }
        public string Source { get; set; }
        public int? TimeElapsed { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public long? DataSize { get; set; }
    }
}
