using NServiceBus.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB18HandlersSortOrder.Sagas
{
    public class SampleSagaData : ContainSagaData
    {
        public String Sample { get; set; }
    }
}
