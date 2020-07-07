using Messages.Commands;
using Messages.Events;
using NServiceBus;
using Subscriber.Services;
using System;
using System.Threading.Tasks;

namespace Subscriber.WebApi.Sagas
{
    public class AddMeasurePolicy : Saga<AddMeasureDataPolicy>,
            IAmStartedByMessages<IMeasureUpdated>,
        IAmStartedByMessages<IUserUpdated>

    {
        private readonly IUserService _userService;

        public AddMeasurePolicy(IUserService userService)
        {
            _userService = userService;
        }

        public Task Handle(IMeasureUpdated message, IMessageHandlerContext context)
        {
            throw new NotImplementedException();
        }

        public Task Handle(IUserUpdated message, IMessageHandlerContext context)
        {
            throw new NotImplementedException();
        }

        public Task Handle(IUpdated message, IMessageHandlerContext context)
        {
            throw new NotImplementedException();
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<AddMeasureDataPolicy> mapper)
        {
            throw new NotImplementedException();
        }
    }
}
