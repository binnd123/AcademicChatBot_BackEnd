using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;

namespace AcademicChatBot.Common.DTOs
{
    public class Response
    {
        public bool IsSucess { get; set; } = true;
        public object Data { get; set; }
        public BusinessCode BusinessCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
