using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.Common.Utils;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using Azure.Core;

namespace AcademicChatBot.Service.Implementation
{
    public class AIChatLogService : IAIChatLogService
    {
        private readonly IGenericRepository<AIChatLog> _aIChatLogRepository;
        private readonly IGenericRepository<Message> _messageRepository;
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IGenericRepository<Major> _majorRepository;
        private readonly IGenericRepository<Program> _programRepository;
        private readonly IGenericRepository<Tool> _toolRepository;
        private readonly IGenericRepository<Material> _materialRepository;
        private readonly IGenericRepository<PrerequisiteConstraint> _prerequisiteConstraintRepository;
        private readonly IGenericRepository<Combo> _comboRepository;
        private readonly IGenericRepository<ComboSubject> _comboSubjectRepository;
        private readonly IGenericRepository<Notification> _notificationRepository;
        private readonly IGenericRepository<ProgramingLearningOutcome> _programingLearningOutcomeRepository;
        private readonly IGenericRepository<ProgramingOutcome> _programingOutcomeRepository;
        private readonly IGenericRepository<CourseLearningOutcome> _courseLearningOutcomeRepository;
        private readonly IGenericRepository<Curriculum> _curriculumRepository;
        private readonly IGenericRepository<SubjectInCurriculum> _subjectInCurriculumRepository;
        private readonly IGenericRepository<POMappingPLO> _pOMappingPLORepository;
        private readonly IGenericRepository<Assessment> _assessmentRepository;
        private readonly IGenericRepository<ToolForSubject> _toolForSubjectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntentDetectorService intentDetectorService;
        private readonly IGeminiAPIService geminiApiService;

        public AIChatLogService(IGenericRepository<AIChatLog> aIChatLogRepository, IGenericRepository<Message> messageRepository, IGenericRepository<Subject> subjectRepository, IGenericRepository<Major> majorRepository, IGenericRepository<Program> programRepository, IGenericRepository<Tool> toolRepository, IGenericRepository<Material> materialRepository, IGenericRepository<PrerequisiteConstraint> prerequisiteConstraintRepository, IGenericRepository<Combo> comboRepository, IGenericRepository<ComboSubject> comboSubjectRepository, IGenericRepository<Notification> notificationRepository, IGenericRepository<ProgramingLearningOutcome> programingLearningOutcomeRepository, IGenericRepository<ProgramingOutcome> programingOutcomeRepository, IGenericRepository<CourseLearningOutcome> courseLearningOutcomeRepository, IGenericRepository<Curriculum> curriculumRepository, IGenericRepository<SubjectInCurriculum> subjectInCurriculumRepository, IGenericRepository<POMappingPLO> pOMappingPLORepository, IGenericRepository<Assessment> assessmentRepository, IGenericRepository<ToolForSubject> toolForSubjectRepository, IUnitOfWork unitOfWork, IIntentDetectorService intentDetectorService, IGeminiAPIService geminiApiService)
        {
            _aIChatLogRepository = aIChatLogRepository;
            _messageRepository = messageRepository;
            _subjectRepository = subjectRepository;
            _majorRepository = majorRepository;
            _programRepository = programRepository;
            _toolRepository = toolRepository;
            _materialRepository = materialRepository;
            _prerequisiteConstraintRepository = prerequisiteConstraintRepository;
            _comboRepository = comboRepository;
            _comboSubjectRepository = comboSubjectRepository;
            _notificationRepository = notificationRepository;
            _programingLearningOutcomeRepository = programingLearningOutcomeRepository;
            _programingOutcomeRepository = programingOutcomeRepository;
            _courseLearningOutcomeRepository = courseLearningOutcomeRepository;
            _curriculumRepository = curriculumRepository;
            _subjectInCurriculumRepository = subjectInCurriculumRepository;
            _pOMappingPLORepository = pOMappingPLORepository;
            _assessmentRepository = assessmentRepository;
            _toolForSubjectRepository = toolForSubjectRepository;
            _unitOfWork = unitOfWork;
            this.intentDetectorService = intentDetectorService;
            this.geminiApiService = geminiApiService;
        }

        private string BuildPrompt(IntentType intent, string userMessage, string contextData)
        {
            //- 💰 Học phí mỗi kỳ: 28,700,000 VND.
            return $@"
            📌 Người dùng vừa hỏi: {userMessage}
            🎯 Ý định đã xác định: {intent}
            📚 Thông tin học tập liên quan:
            {contextData}

            Bạn là một chuyên gia tư vấn học thuật giàu kinh nghiệm của đại học FPT, hiểu rõ về các ngành học, chuyên ngành, lộ trình học đại học, kỹ năng nghề nghiệp và xu hướng thị trường lao động.

            🎓 Dưới đây là một số thông tin quan trọng về trường đại học FPT:
            - 📅 Một năm học có 3 kỳ: Spring, Summer, Fall (ví dụ: Spring2025).
            - ⏳ Tổng thời gian học: 4 năm bao gồm 1 năm tiếng Anh dự bị và 3 năm học chuyên ngành. Nếu có chứng chỉ IELTS từ 6.0 trở lên, sinh viên có thể bỏ qua năm học tiếng Anh dự bị và học thẳng chuyên ngành.
            - 🧭 Trong 4 kỳ đầu, tất cả sinh viên học giống nhau. Từ cuối kỳ 4 (đầu kỳ 5), sinh viên phải chọn combo chuyên ngành (ví dụ: .NET, Java, SAP...), các môn sau đó sẽ thay đổi theo combo.
            - 📘 Mỗi kỳ có 5 môn học: 4 môn học trực tiếp tại trường và 1 môn Coursera học online (có ký hiệu 'c' ở cuối mã môn, ví dụ: SSL101c).
            - 🏢 Kỳ 6 đặc biệt: học 1 môn Coursera và đi thực tập OJT tại doanh nghiệp thay cho 4 môn trên lớp.
            - 🎓 Kỳ 9: thay môn Coursera bằng đồ án tốt nghiệp (Capstone Project).

            🎯 Hãy đưa ra lời khuyên và định hướng học tập phù hợp dựa trên nội dung trên. Lưu ý:

            1. Sử dụng emoji để tạo cảm giác thân thiện và dễ hiểu (ví dụ: 🎯, 💡, 👨‍🎓, ✅…).
            2. Giọng văn gần gũi, rõ ràng nhưng vẫn mang tính chuyên môn. Có thể đan xen một vài câu hài hước nhẹ nhàng để bớt khô khan 😄.
            3. Tránh đưa ra thông tin sai lệch hoặc mơ hồ. Hạn chế sử dụng ngôn ngữ quá trừu tượng.
            4. Ưu tiên ví dụ thực tế hoặc gợi ý cụ thể (ví dụ: nếu chọn chuyên ngành A thì có thể học combo B, phù hợp với sinh viên thích kiểu học như…).
            5. Trình bày mạch lạc, có thể chia theo các đề mục hoặc bước tư vấn rõ ràng.
            6. Giữ độ dài trong khoảng 20 câu để người dùng dễ tiếp thu.

            Bắt đầu nhé! 🎉
            ";
        }


        public async Task<Response> GenerateResponseAsync(Guid? userId, string message)
        {
            Response dto = new Response();
            try
            {
                // Bước 1: Phân tích nội dung tin nhắn để xác định ý định
                var intent = await intentDetectorService.DetectAsync(message);

                // Bước 2: Thu thập dữ liệu cần thiết từ DB hoặc ngữ cảnh
                string contextData = string.Empty;

                switch (intent)
                {
                    case IntentType.AskMajorAdvice:
                        {
                            var majors = await _majorRepository.GetAllDataByExpression(
                                filter: c => !c.IsDeleted
                                , pageNumber: 1
                                , pageSize: int.MaxValue
                                , orderBy: m => m.MajorName
                                , isAscending: true
                                , includes: null); // Lấy danh sách ngành
                            var combo = await _comboRepository.GetAllDataByExpression(
                                filter: c => !c.IsDeleted
                                , pageNumber: 1
                                , pageSize: int.MaxValue
                                , orderBy: m => m.ComboName
                                , isAscending: true
                                , includes: null); // Lấy danh sách combo chuyên sâu
                            var curriculum = await _curriculumRepository.GetAllDataByExpression(
                                filter: c => !c.IsDeleted
                                , pageNumber: 1
                                , pageSize: int.MaxValue
                                , orderBy: m => m.CurriculumName
                                , isAscending: true
                                , includes: null); // Lấy danh sách chương trình đào tạo
                            contextData = JsonSerializerHelper.SerializeData(new
                            {
                                majors = majors.Items,
                                combo = combo.Items,
                                curriculum = curriculum.Items
                            });
                            break;
                        }

                    case IntentType.AskSpecializationCombo:
                        {
                            var combos = await _comboSubjectRepository.GetAllDataByExpression(
                                filter: null,
                                pageNumber: 1,
                                pageSize: int.MaxValue,
                                orderBy: null,
                                isAscending: true,
                                includes: x => x.Combo); // Lấy danh sách tổ hợp
                            contextData = JsonSerializerHelper.SerializeData(combos.Items);
                            break;
                        }

                    case IntentType.AskCurriculum:
                        {
                            var curriculums = await _curriculumRepository.GetAllDataByExpression(
                                filter: c => !c.IsDeleted,
                                pageNumber: 1,
                                pageSize: int.MaxValue,
                                orderBy: null,
                                isAscending: true,
                                includes: null); // Lấy danh sách chương trình học
                            contextData = JsonSerializerHelper.SerializeData(curriculums.Items);
                            break;
                        }

                    //case IntentType.AskStudyPlan:
                    //    {
                    //        var curriculum = await _curriculumRepository.GetAllDataByExpression(
                    //            filter: null, pageNumber: 1, pageSize: 50, orderBy: c => c.CurriculumName, isAscending: true, includes: c => c.Major);
                    //        var subjects = await _subjectInCurriculumRepository.GetAllDataByExpression(
                    //            filter: null, pageNumber: 1, pageSize: 200, orderBy: s => s.Semester, isAscending: true, includes: s => s.Subject);
                    //        contextData = JsonSerializerHelper.SerializeData(new { curriculum, subjects });
                    //        break;
                    //    }

                    case IntentType.CompareSemesterProgress:
                        {
                            var subjectInCurriculums = await _subjectInCurriculumRepository.GetAllDataByExpression(
                                filter: null, pageNumber: 1, pageSize: int.MaxValue, orderBy: s => s.SemesterNo, isAscending: true, includes: s => s.Subject);
                            contextData = JsonSerializerHelper.SerializeData(subjectInCurriculums.Items);
                            break;
                        }

                    //case IntentType.AskInternshipAdvice:
                    //    {
                    //        var internshipSubjects = await _subjectRepository.GetAllDataByExpression(
                    //            s => s.SubjectName.Contains("thực tập") || s.SubjectName.Contains("internship"), pageNumber: 1, pageSize: 50,
                    //            orderBy: s => s.SubjectName, isAscending: true, includes: null);
                    //        contextData = JsonSerializerHelper.SerializeData(internshipSubjects);
                    //        break;
                    //    }

                    case IntentType.AskCourseraStrategy:
                        {
                            var courseraSubjects = await _subjectRepository.GetAllDataByExpression(
                                s => s.SubjectName.Contains("Coursera"), pageNumber: 1, pageSize: int.MaxValue,
                                orderBy: s => s.SubjectName, isAscending: true, includes: null);
                            contextData = JsonSerializerHelper.SerializeData(courseraSubjects);
                            break;
                        }

                    default:
                        {
                            contextData = "";
                            break;
                        }
                }

                // Bước 3: Tạo prompt gửi đến AI
                var finalPrompt = message;
                if (!string.IsNullOrEmpty(contextData))
                {
                    finalPrompt = BuildPrompt(intent, message, contextData);
                }

                // Bước 4: Gọi API Gemini (hoặc dịch vụ AI khác)
                var geminiResponse = await geminiApiService.GenerateResponseAsync(finalPrompt);

                // Bước 5: Trả kết quả về cho client
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = geminiResponse;
                dto.Message = "Generate AI chat log response successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating AI promp: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAIChatLogById(Guid? userId, Guid aIChatLogId)
        {
            Response dto = new Response();
            try
            {
                var chat = await _aIChatLogRepository.GetFirstByExpression(
                    filter: a => a.UserId == userId && a.AIChatLogId == aIChatLogId);

                if (chat == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Chat log not found.";
                    return dto;
                }
                var message = await _messageRepository.GetAllDataByExpression(
                    filter: m => m.AIChatLogId == chat.AIChatLogId,
                    pageNumber: 1,
                    pageSize: 20,
                    orderBy: m => m.SentTime,
                    isAscending: false,
                    includes: null);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Data = new
                {
                    AIChatLog = chat,
                    Messages = message
                };
                dto.Message = "Retrieved chat log successfully.";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the chat log: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAIChatLogActivedByUserId(Guid? userId)
        {
            Response dto = new Response();
            try
            {
                var aIChatLog = await _aIChatLogRepository.GetFirstByExpression(
                    filter: a => a.UserId == userId && a.IsDeleted == false && a.Status == StatusChat.Actived && a.EndTime == null);
                if (aIChatLog == null)
                {
                    dto.IsSucess = true;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "AI chat log not found";
                    return dto;
                }
                var message = await _messageRepository.GetAllDataByExpression(
                    filter: m => m.AIChatLogId == aIChatLog.AIChatLogId,
                    pageNumber: 1,
                    pageSize: 20,
                    orderBy: m => m.SentTime,
                    isAscending: false,
                    includes: null);
                dto.Data = new
                {
                    AIChatLog = aIChatLog,
                    Messages = message
                };
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "AI chat log retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the chat log: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllAIChatLogByUserId(Guid? userId, int pageNumber, int pageSize, bool isDelete)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _aIChatLogRepository.GetAllDataByExpression(
                    filter: a => a.UserId == userId && a.IsDeleted == isDelete,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: a => a.CreatedAt,
                    isAscending: true,
                    includes: null);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "AI chat log retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the chat log: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> UpdateAIChatLog(Guid? userId, Guid aIChatLogId, StatusChat status)
        {
            Response dto = new Response();
            try
            {
                var chat = await _aIChatLogRepository.GetFirstByExpression(
                    filter: a => a.UserId == userId && a.AIChatLogId == aIChatLogId);

                if (chat == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Chat log not found.";
                    return dto;
                }

                chat.Status = status;
                chat.UpdatedAt = DateTime.Now;

                await _aIChatLogRepository.Update(chat);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Message = "AI chat log update successfully";
                dto.Data = chat;
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while update the chat log: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteAIChatLog(Guid? userId, Guid aIChatLogId)
        {
            Response dto = new Response();
            try
            {
                var chat = await _aIChatLogRepository.GetFirstByExpression(
                    filter: a => a.UserId == userId && a.AIChatLogId == aIChatLogId);

                if (chat == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Chat log not found.";
                    return dto;
                }

                chat.IsDeleted = true;
                chat.DeletedAt = DateTime.Now;
                chat.UpdatedAt = DateTime.Now;
                chat.EndTime = DateTime.Now;

                await _aIChatLogRepository.Update(chat);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Message = "AI chat log delete successfully";
                dto.Data = chat;
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while delete the chat log: " + ex.Message;
            }
            return dto;
        }
    }
}
