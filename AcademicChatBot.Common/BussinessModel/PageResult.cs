﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.DTOs
{
    public class PageResult<T> where T : class
    {
        public List<T>? Items { get; set; }
        public int TotalPages { get; set; }
    }
}
