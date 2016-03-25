using NServiceBus;
using NServiceBus.Sagas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSB07Testing.Messages;

namespace NSB07Testing.Sagas
{
    public class AppTypePolicy : Saga<AppTypePolicyData>,
        IAmStartedByMessages<Messages.AppTypeRequest>,
        IAmStartedByMessages<Messages.SetAppType>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<AppTypePolicyData> mapper)
        {
            mapper.ConfigureMapping<Messages.SetAppType>(r => r.AppId).ToSaga(r => r.AppId);
            mapper.ConfigureMapping<Messages.AppTypeRequest>(r => r.AppId).ToSaga(r => r.AppId);
        }

        public async Task Handle(AppTypeRequest message, IMessageHandlerContext context)
        {
            this.Data.AppId = message.AppId;

            var response = new Messages.AppTypeResponse();
            response.AppId = this.Data.AppId;
            response.AppType = this.Data.AppType;

            await context.Reply(response);
        }

        public Task Handle(SetAppType message, IMessageHandlerContext context)
        {
            this.Data.AppId = message.AppId;
            this.Data.AppType = Messages.AppType.Known;

            return Task.FromResult(false);
        }
    }


    public class AppTypePolicyData : ContainSagaData
    {
        public int AppId { get; set; }
        public Messages.AppType AppType { get; set; }

        public AppTypePolicyData()
        {
            this.AppType = Messages.AppType.Unknown;
        }
    }

}
