using FireOnWheels.MessageContracts;

namespace FireOnWheels.Registration.Service.Messages
{
    public class OrderRegisteredEvent : IOrderRegisteredEvent
    {
        public int OrderId { get; set; }

        public string PickupName { get; set; }

        public string PickupAddress { get; set; }

        public string PickupCity { get; set; }

        public string DeliverName { get; set; }

        public string DeliverAddress { get; set; }

        public string DeliverCity { get; set; }

        public int Weight { get; set; }

        public bool Fragile { get; set; }

        public bool Oversized { get; set; }

        public OrderRegisteredEvent(IRegisterOrderCommand command, int id)
        {
            this.OrderId = id;
            this.PickupName = command.PickupName;
            this.PickupAddress = command.PickupAddress;
            this.PickupCity = command.PickupCity;
            this.DeliverName = command.DeliverName;
            this.DeliverAddress = command.DeliverAddress;
            this.DeliverCity = command.DeliverCity;
            this.Weight = command.Weight;
            this.Fragile = command.Fragile;
            this.Oversized = command.Oversized;
        }
    }
}
