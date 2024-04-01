using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.EventStore.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string AggregateId { get; set; }
        public string DataJson { get; set; }
        public string DataType { get; set; }
        public DateTime EventTime { get; set; }
        public bool IsCompleted { get; set; }

    }
}
