using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel.Assessment;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Contract
{
    public interface IAssessmentService
    {
        Task<Response> CreateAssessment(CreateAssessmentRequest request);
        Task<Response> DeleteAssessment(Guid assessmentId);
        Task<Response> GetAllAssessments(int pageNumber, int pageSize, string search, bool isDelete);
        Task<Response> GetAssessmentById(Guid assessmentId);
        Task<Response> UpdateAssessment(Guid assessmentId, UpdateAssessmentRequest request);
    }
}
