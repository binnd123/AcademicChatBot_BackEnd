using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.DBContext;

namespace AcademicChatBot.DAL.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private AcademicChatBotDBContext _context;

        public UnitOfWork(AcademicChatBotDBContext context)
        {
            _context = context;
        }


        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
