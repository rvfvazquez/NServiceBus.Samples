using NServiceBus.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB18HandlersSortOrder.Sagas
{
    public class SampleSaga :
        Saga<SampleSagaData>,
        IAmStartedByMessages<Commands.StartSagaCommand>
    {
        protected override void ConfigureHowToFindSaga( SagaPropertyMapper<SampleSagaData> mapper )
        {
            
        }

        public void Handle( Commands.StartSagaCommand message )
        {
            this.MarkAsComplete();
        }
    }
}
