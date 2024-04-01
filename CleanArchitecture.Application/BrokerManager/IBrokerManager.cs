using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Commands.BrokerManager
{
    public interface IBrokerManager
    {
        public void publish(object data, string eventType);
        public void consume();
    }
}
