﻿using Liquid.Messaging.Tests.Mock;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Messaging.Tests
{
    public class LiquidConsumerBaseTest : LiquidConsumerBase<EntityMock>
    {
        private static readonly ILiquidConsumer<EntityMock> _consumer = Substitute.For<ILiquidConsumer<EntityMock>>();

        public LiquidConsumerBaseTest() : base(_consumer)
        {
        }

        [Fact]
        public void ExecuteAsync_WhenStart_ConsumerReceivedStartCall()
        {
            var task = base.ExecuteAsync(new CancellationToken());

            task.Wait(1000);

            _consumer.Received().RegisterMessageHandler();
        }


        [Fact]
        public async Task ExecuteAsync_WhenStartFail_ThrowException()
        {
            _consumer.When(x =>
            x.RegisterMessageHandler())
                .Do((call) => throw new Exception());

            var task = ExecuteAsync(new CancellationToken());

            await Assert.ThrowsAsync<Exception>(() => task);
        }

        public override Task ProcessMessageAsync(ProcessMessageEventArgs<EntityMock> args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
