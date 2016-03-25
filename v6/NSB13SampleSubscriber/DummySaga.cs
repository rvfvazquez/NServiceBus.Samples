using NServiceBus.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB13SampleSubscriber
{
    public class DummySaga : Saga<DummySaga.State>
    {
        public class State : ContainSagaData { }

        protected override void ConfigureHowToFindSaga( SagaPropertyMapper<DummySaga.State> mapper )
        {

        }
    }
}
