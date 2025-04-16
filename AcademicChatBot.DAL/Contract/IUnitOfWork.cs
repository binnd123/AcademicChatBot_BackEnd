﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Contract
{
    public interface IUnitOfWork
    {
        public Task<int> SaveChangeAsync();
    }
}
