using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.DTOs.BussinessCode;

namespace AcademicChatBot.Common.DTOs
{
    public class ResponseDTO
    {
        public bool IsSucess { get; set; } = true;
        public object Data { get; set; }
        public BusinessCode BusinessCode { get; set; }
    }
}
