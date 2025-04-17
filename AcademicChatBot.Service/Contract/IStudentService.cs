using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Students;

namespace AcademicChatBot.Service.Contract
{
    public interface IStudentService
    {
        public Task<ResponseDTO> GetStudentProfile(Guid studentId);
        public Task<ResponseDTO> UpdateStudentProfile(Guid studentId, StudentProfileRequest request);
    }
}
