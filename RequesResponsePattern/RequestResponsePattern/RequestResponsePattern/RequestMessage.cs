using System;
using System.Collections.Generic;
using System.Text;

namespace RequestResponsePattern
{
    public class RequestMessage : IRequestMessage
    {
        public int Id { get; set; }

        public string Content { get; set; }
    }
}
