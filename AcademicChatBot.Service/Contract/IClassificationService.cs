using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Service.Contract
{
    public interface IClassificationService
    {
        Task<int> ClassificationIntent(string message);
    }
}
