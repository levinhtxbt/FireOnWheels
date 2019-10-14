using System;
using System.Collections.Generic;
using System.Text;

namespace RequestResponsePattern
{
    public class ResponseMessage : IResponseMessage
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsSuccess { get; set; }
    }
}
