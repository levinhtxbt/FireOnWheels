﻿using Automatonymous;
using FireOnWheels.MessageContracts;
using System;


namespace FireOnWheels.Saga
{
    public class OrderSaga : MassTransitStateMachine<OrderSagaState>
    {
        public State Received { get; private set; }

        public State Registered { get; private set; }

        public Event<IRegisterOrderCommand> RegisterOrder { get; private set; }

        public Event<IOrderRegisteredEvent> OrderRegistered { get; private set; }

        public OrderSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => RegisterOrder, 
                cc => cc.CorrelateBy(state => state.PickupName, context => context.Message.PickupName).SelectId(context => Guid.NewGuid()));

            Event(() => OrderRegistered, x => x.CorrelateById(c => c.Message.CorrelationId));

            Initially(When(RegisterOrder)
                .Then(context =>
                {
                    //context.Instance.CorrelationId = context.Data;
                    //context.Instance.OrderId = context.Data.OrderId;
                    context.Instance.PickupName = context.Data.PickupName;
                    context.Instance.PickupCity = context.Data.PickupCity;
                    context.Instance.PickupAddress = context.Data.PickupAddress;
                    context.Instance.DeliverName = context.Data.DeliverName;
                    context.Instance.DeliverAddress = context.Data.DeliverAddress;
                    context.Instance.DeliverCity = context.Data.DeliverCity;
                    context.Instance.Weight = context.Data.Weight;
                    context.Instance.Fragile = context.Data.Fragile;
                    context.Instance.Oversized = context.Data.Oversized;
                })
                .ThenAsync(context => Console.Out.WriteLineAsync($"Order for customer {context.Instance.PickupName} received"))
                .TransitionTo(Received)
                .Publish(context => new OrderReceivedEvent(context.Instance)));

            During(Received, When(OrderRegistered)
                .Then(context => context.Instance.RegisterdDateTime = DateTime.Now)
                .ThenAsync(context => Console.Out.WriteLineAsync($"Order for customer {context.Instance.PickupName} registered"))
                .Finalize());

            SetCompletedWhenFinalized();
        }
    }
}
