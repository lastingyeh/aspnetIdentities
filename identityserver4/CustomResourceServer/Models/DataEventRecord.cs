using System;

namespace CustomResourceServer.Models
{
    public class DataEventRecord
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
