using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.AIChatLogs;
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
        private readonly IGenericRepository<Combo> _comboRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntentDetectorService intentDetectorService;
        private readonly IGeminiAPIService geminiApiService;

        public AIChatLogService(IGenericRepository<AIChatLog> aIChatLogRepository, IGenericRepository<Message> messageRepository, IGenericRepository<Subject> subjectRepository, IGenericRepository<Major> majorRepository, IGenericRepository<Program> programRepository, IGenericRepository<Tool> toolRepository, IGenericRepository<Material> materialRepository, IGenericRepository<Combo> comboRepository, IUnitOfWork unitOfWork, IIntentDetectorService intentDetectorService, IGeminiAPIService geminiApiService)
        {
            _aIChatLogRepository = aIChatLogRepository;
            _messageRepository = messageRepository;
            _subjectRepository = subjectRepository;
            _majorRepository = majorRepository;
            _programRepository = programRepository;
            _toolRepository = toolRepository;
            _materialRepository = materialRepository;
            _comboRepository = comboRepository;
            _unitOfWork = unitOfWork;
            this.intentDetectorService = intentDetectorService;
            this.geminiApiService = geminiApiService;
        }



        //public async Task<Guid> AddChatAsync(Guid userId, AIChatLogRequest chatRequest)
        //{
        //        var messageDomain = new AIChatLog
        //        {
        //            AIChatLogId = Guid.NewGuid(),
        //            UserId = userId,
        //            IsDeleted = false,
        //            LastMessageTime = chatRequest.LastMessageTime,
        //            StartTime = DateTime.UtcNow,
        //            CreatedAt = DateTime.UtcNow,
        //            UpdatedAt = DateTime.UtcNow,
        //        };
        //        // Use Domain Model to create Author
        //        await _aIChatLogRepository.Insert(messageDomain);
        //        await _unitOfWork.SaveChangeAsync();
        //        return messageDomain.AIChatLogId;
        //}

        private string BuildPrompt(IntentType intent, string userMessage, string contextData)
        {
            return $@"
📌 Người dùng vừa hỏi: {userMessage}
🎯 Ý định đã xác định: {intent}
📚 Thông tin học tập liên quan:
{contextData}

Bạn là một chuyên gia tư vấn học thuật giàu kinh nghiệm, hiểu rõ về các ngành học, chuyên ngành, lộ trình học đại học, kỹ năng nghề nghiệp và xu hướng thị trường lao động.

🎓 Hãy đưa ra lời khuyên và định hướng học tập phù hợp dựa trên nội dung trên. Lưu ý:

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
                                filter: null
                                , pageNumber: 1
                                , pageSize: 100
                                , orderBy: m => m.MajorName
                                , isAscending: true
                                , includes: null); // Lấy danh sách ngành
                            contextData = JsonSerializerHelper.SerializeData(majors);
                            break;
                        }

                    case IntentType.AskSpecializationCombo:
                        {
                            var combos = await _comboRepository.GetAllDataByExpression(
                                filter: null
                                , pageNumber: 1
                                , pageSize: 100
                                , orderBy: c => c.ComboName
                                , isAscending: true
                                , includes: c => c.Major); // Lấy danh sách tổ hợp
                            contextData = JsonSerializerHelper.SerializeData(combos);
                            break;
                        }

                    //case IntentType.AskStudyPlan:
                    //    {
                    //        var semesters = await semesterRepository.GetAllByUserAsync(userId); // Lấy dữ liệu học kỳ của sinh viên
                    //        var dto = mapper.Map<List<SemesterPlanBotRequest>>(semesters);
                    //        contextData = JsonSerializerHelper.SerializeData(dto);
                    //        break;
                    //    }

                    case IntentType.AskElectiveSubjects:
                        {
                            var electives = await _subjectRepository.GetAllDataByExpression(
                                filter: null
                                , pageNumber: 1
                                , pageSize: 100
                                , orderBy: s => s.SubjectName
                                , isAscending: true
                                , includes: s => s.Curriculum);
                            contextData = JsonSerializerHelper.SerializeData(electives);
                            break;
                        }

                    //case IntentType.AskCapstoneAdvice:
                    //    {
                    //        var capstoneData = await capstoneRepository.GetByUserAsync(userId);
                    //        var dto = mapper.Map<CapstoneBotRequest>(capstoneData);
                    //        contextData = JsonSerializerHelper.SerializeData(dto);
                    //        break;
                    //    }

                    //case IntentType.CompareSemesterProgress:
                    //    {
                    //        var comparisonData = await semesterRepository.GetProgressComparison(userId);
                    //        contextData = JsonSerializerHelper.SerializeData(comparisonData);
                    //        break;
                    //    }

                    //case IntentType.AskInternshipAdvice:
                    //    {
                    //        var internship = await internshipRepository.GetByUserAsync(userId);
                    //        var dto = mapper.Map<InternshipBotRequest>(internship);
                    //        contextData = JsonSerializerHelper.SerializeData(dto);
                    //        break;
                    //    }

                    //case IntentType.AskCourseraStrategy:
                    //    {
                    //        var courseraProgress = await courseraRepository.GetByUserAsync(userId);
                    //        var dto = mapper.Map<List<CourseraBotRequest>>(courseraProgress);
                    //        contextData = JsonSerializerHelper.SerializeData(dto);
                    //        break;
                    //    }

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
                chat.UpdatedAt = DateTime.UtcNow;

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
                chat.DeletedAt = DateTime.UtcNow;
                chat.UpdatedAt = DateTime.UtcNow;
                chat.EndTime = DateTime.UtcNow;

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
