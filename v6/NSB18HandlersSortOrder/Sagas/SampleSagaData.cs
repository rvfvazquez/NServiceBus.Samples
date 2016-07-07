using NServiceBus;
using System;

namespace NSB18HandlersSortOrder.Sagas
{
    public class SampleSagaData : ContainSagaData
    {
        public String Sample { get; set; }
    }
}
