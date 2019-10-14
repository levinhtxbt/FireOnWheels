using System;
using System.Collections.Generic;
using System.Text;

namespace RequestResponsePattern
{
    public interface IRequestMessage
    {
        public int Id { get; set; }

        public string Content { get; set; }
    }
}
