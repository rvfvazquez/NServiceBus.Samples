using NServiceBus;

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
