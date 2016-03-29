using NServiceBus;
using System.Threading.Tasks;
using NSB18HandlersSortOrder.Commands;

namespace NSB18HandlersSortOrder.Sagas
{
    public class SampleSaga :
        Saga<SampleSagaData>,
        IAmStartedByMessages<StartSagaCommand>
    {
        protected override void ConfigureHowToFindSaga( SagaPropertyMapper<SampleSagaData> mapper )
        {
            
        }

        public Task Handle(StartSagaCommand message, IMessageHandlerContext context)
        {
            this.MarkAsComplete();

            return Task.CompletedTask;
        }
    }
}
