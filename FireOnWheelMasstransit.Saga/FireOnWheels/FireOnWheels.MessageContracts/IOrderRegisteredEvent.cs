using System;
using System.Collections.Generic;
using System.Text;

namespace FireOnWheels.MessageContracts
{
    public interface IOrderRegisteredEvent
    {
        Guid CorrelationId { get; set; }
        int OrderId { get; }
        string PickupName { get; }
        string PickupAddress { get; }
        string PickupCity { get; }

        string DeliverName { get; }
        string DeliverAddress { get; }
        string DeliverCity { get; }

        int Weight { get; }
        bool Fragile { get; }
        bool Oversized { get; }
    }
}
