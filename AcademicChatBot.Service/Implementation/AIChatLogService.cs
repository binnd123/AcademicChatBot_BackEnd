using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
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

namespace AcademicChatBot.Service.Implementation
{
    public class AIChatLogService : IAIChatLogService
    {
        private readonly IGenericRepository<AIChatLog> _aIChatLogRepository;
        private readonly IGenericRepository<Message> _messageRepository;
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IGenericRepository<Major> _majorRepository;
        private readonly IGenericRepository<Program> _programRepository;
        private readonly IGenericRepository<PrerequisiteSubject> _prerequisiteSubjectRepository;
        private readonly IGenericRepository<PrerequisiteConstraint> _prerequisiteConstraintRepository;
        private readonly IGenericRepository<Combo> _comboRepository;
        private readonly IGenericRepository<ComboSubject> _comboSubjectRepository;
        private readonly IGenericRepository<ProgramingOutcome> _programingOutcomeRepository;
        private readonly IGenericRepository<CourseLearningOutcome> _courseLearningOutcomeRepository;
        private readonly IGenericRepository<Curriculum> _curriculumRepository;
        private readonly IGenericRepository<SubjectInCurriculum> _subjectInCurriculumRepository;
        private readonly IGenericRepository<Assessment> _assessmentRepository;
        private readonly IGenericRepository<Material> _materialRepository;
        private readonly IGenericRepository<ToolForSubject> _toolForSubjectRepository;
        private readonly IGenericRepository<Tool> _toolRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntentDetectorService intentDetectorService;
        private readonly IGeminiAPIService geminiApiService;
        private readonly IPrerequisiteSubjectService _prerequisiteSubjectService;

        public AIChatLogService(IGenericRepository<AIChatLog> aIChatLogRepository, IGenericRepository<Message> messageRepository, IGenericRepository<Subject> subjectRepository, IGenericRepository<Major> majorRepository, IGenericRepository<Program> programRepository, IGenericRepository<PrerequisiteSubject> prerequisiteSubjectRepository, IGenericRepository<PrerequisiteConstraint> prerequisiteConstraintRepository, IGenericRepository<Combo> comboRepository, IGenericRepository<ComboSubject> comboSubjectRepository, IGenericRepository<ProgramingOutcome> programingOutcomeRepository, IGenericRepository<CourseLearningOutcome> courseLearningOutcomeRepository, IGenericRepository<Curriculum> curriculumRepository, IGenericRepository<SubjectInCurriculum> subjectInCurriculumRepository, IGenericRepository<Assessment> assessmentRepository, IGenericRepository<Material> materialRepository, IGenericRepository<ToolForSubject> toolForSubjectRepository, IGenericRepository<Tool> toolRepository, IUnitOfWork unitOfWork, IIntentDetectorService intentDetectorService, IGeminiAPIService geminiApiService, IPrerequisiteSubjectService prerequisiteSubjectService)
        {
            _aIChatLogRepository = aIChatLogRepository;
            _messageRepository = messageRepository;
            _subjectRepository = subjectRepository;
            _majorRepository = majorRepository;
            _programRepository = programRepository;
            _prerequisiteSubjectRepository = prerequisiteSubjectRepository;
            _prerequisiteConstraintRepository = prerequisiteConstraintRepository;
            _comboRepository = comboRepository;
            _comboSubjectRepository = comboSubjectRepository;
            _programingOutcomeRepository = programingOutcomeRepository;
            _courseLearningOutcomeRepository = courseLearningOutcomeRepository;
            _curriculumRepository = curriculumRepository;
            _subjectInCurriculumRepository = subjectInCurriculumRepository;
            _assessmentRepository = assessmentRepository;
            _materialRepository = materialRepository;
            _toolForSubjectRepository = toolForSubjectRepository;
            _toolRepository = toolRepository;
            _unitOfWork = unitOfWork;
            this.intentDetectorService = intentDetectorService;
            this.geminiApiService = geminiApiService;
            _prerequisiteSubjectService = prerequisiteSubjectService;
        }

        //private string BuildPrompt(IntentType intent, string userMessage, string contextData)
        //{
        //    //- 💰 Học phí mỗi kỳ: 28,700,000 VND.
        //    return $@"
        //    📌 Người dùng vừa hỏi: {userMessage}
        //    🎯 Ý định đã xác định: {intent}
        //    📚 Thông tin học tập liên quan:
        //    {contextData}

        //    Bạn là một chuyên gia tư vấn học thuật giàu kinh nghiệm của đại học FPT, hiểu rõ về các ngành học, chuyên ngành, lộ trình học đại học, kỹ năng nghề nghiệp và xu hướng thị trường lao động.

        //    🎓 Dưới đây là một số thông tin quan trọng về trường đại học FPT:
        //    - 📅 Một năm học có 3 kỳ: Spring, Summer, Fall (ví dụ: Spring2025).
        //    - ⏳ Tổng thời gian học: 4 năm bao gồm 1 năm tiếng Anh dự bị và 3 năm học chuyên ngành. Nếu có chứng chỉ IELTS từ 6.0 trở lên, sinh viên có thể bỏ qua năm học tiếng Anh dự bị và học thẳng chuyên ngành.
        //    - 🧭 Trong 4 kỳ đầu, tất cả sinh viên học giống nhau. Từ cuối kỳ 4 (đầu kỳ 5), sinh viên phải chọn combo chuyên ngành (ví dụ: .NET, Java, SAP...), các môn sau đó sẽ thay đổi theo combo.
        //    - 📘 Mỗi kỳ có 5 môn học: 4 môn học trực tiếp tại trường và 1 môn Coursera học online (có ký hiệu 'c' ở cuối mã môn, ví dụ: SSL101c).
        //    - 🏢 Kỳ 6 đặc biệt: học 1 môn Coursera và đi thực tập OJT tại doanh nghiệp thay cho 4 môn trên lớp.
        //    - 🎓 Kỳ 9: thay môn Coursera bằng đồ án tốt nghiệp (Capstone Project).

        //    🎯 Hãy đưa ra lời khuyên và định hướng học tập phù hợp dựa trên nội dung trên. Lưu ý:

        //    1. Sử dụng emoji để tạo cảm giác thân thiện và dễ hiểu (ví dụ: 🎯, 💡, 👨‍🎓, ✅…).
        //    2. Giọng văn gần gũi, rõ ràng nhưng vẫn mang tính chuyên môn. Có thể đan xen một vài câu hài hước nhẹ nhàng để bớt khô khan 😄.
        //    3. Tránh đưa ra thông tin sai lệch hoặc mơ hồ. Hạn chế sử dụng ngôn ngữ quá trừu tượng.
        //    4. Ưu tiên ví dụ thực tế hoặc gợi ý cụ thể (ví dụ: nếu chọn chuyên ngành A thì có thể học combo B, phù hợp với sinh viên thích kiểu học như…).
        //    5. Trình bày mạch lạc, có thể chia theo các đề mục hoặc bước tư vấn rõ ràng.
        //    6. Giữ độ dài trong khoảng 20 câu để người dùng dễ tiếp thu.

        //    Bắt đầu nhé! 🎉
        //    ";
        //}

        private string BuildPrompt(IntentType intent, string userMessage, string contextData)
        {
            string intro = $@"
            📌 Người dùng vừa hỏi: {userMessage}
            🎯 Ý định đã xác định: {intent}
            📚 Thông tin học tập liên quan:
            {contextData}

            Bạn là một chuyên gia tư vấn học thuật giàu kinh nghiệm của Đại học FPT, hiểu rõ về các ngành học, chuyên ngành, lộ trình học đại học, kỹ năng nghề nghiệp và xu hướng thị trường lao động.";

            string commonInfo = @"
            🎓 Một số thông tin chung về Đại học FPT:
            - 📅 Một năm học có 3 kỳ: Spring, Summer, Fall (ví dụ: Spring2025).
            - ⏳ Tổng thời gian học: 4 năm (1 năm tiếng Anh dự bị + 3 năm chuyên ngành). Có IELTS 6.0+ có thể bỏ qua tiếng Anh dự bị.
            - 🧭 4 kỳ đầu học giống nhau, từ kỳ 5 sinh viên chọn combo chuyên ngành (.NET, Java, SAP...).
            - 📘 Mỗi kỳ 5 môn: 4 môn học tại trường, 1 môn Coursera (mã môn có 'c' ở cuối, ví dụ SSL101c).
            - 🏢 Kỳ 6: 1 môn Coursera + thực tập OJT.
            - 🎓 Kỳ 9: đồ án tốt nghiệp (Capstone Project) thay cho môn Coursera.";

            string guidance = @"
            🎯 Hãy trả lời ngắn gọn, rõ ràng và đúng trọng tâm câu hỏi. Lưu ý:
            1. Ưu tiên cung cấp thông tin thật sự cần thiết để sinh viên hiểu và ra quyết định.
            2. Tránh lan man, suy diễn hoặc đưa thông tin không liên quan.
            3. Độ dài tối ưu: 5–10 câu. Ưu tiên chất lượng hơn số lượng.
            4. Trình bày mạch lạc, có thể chia đề mục nếu cần thiết.
            5. Chỉ đưa ví dụ minh họa nếu câu hỏi yêu cầu rõ ràng.
            6. Sử dụng emoji hợp lý để tạo cảm giác thân thiện và dễ hiểu (🎯, 💡, 👨‍🎓, ✅…).
            7. Giữ giọng văn gần gũi, chuyên môn; tránh ngôn ngữ trừu tượng hoặc hài hước quá mức.

            Bắt đầu nhé! 🎉";


            string topicSpecificPrompt = intent switch
            {
                IntentType.AskMajorAdvice => @"
            💬 Chủ đề trọng tâm: Tư vấn chọn ngành học
            - Giúp sinh viên xác định ngành học phù hợp với sở thích, năng lực và xu hướng nghề nghiệp.
            - Nếu phù hợp, đưa ra ví dụ các ngành hot như Công nghệ thông tin, Kinh doanh quốc tế, Thiết kế đồ họa... 👨‍🎓💡
            - Khuyến khích sinh viên suy nghĩ về điểm mạnh cá nhân và định hướng nghề nghiệp dài hạn.",

                IntentType.AskSpecializationCombo => @"
            💬 Chủ đề trọng tâm: Tư vấn chọn combo chuyên ngành
            - Giải thích combo chuyên ngành là gì và ảnh hưởng thế nào đến môn học sau này.
            - Gợi ý combo phù hợp với từng định hướng nghề nghiệp (ví dụ: muốn làm Dev backend thì chọn .NET/Java, thích phân tích dữ liệu thì chọn Data Science...). 📈👨‍💻",

                IntentType.AskProgram => @"
            💬 Chủ đề trọng tâm: Tư vấn về chương trình đào tạo
            - Mô tả rõ tổng thể chương trình từ tiếng Anh dự bị đến chuyên ngành.
            - Giải thích kỳ OJT (On the Job Training) và Capstone Project giúp sinh viên chuẩn bị cho thị trường lao động thực tế. 🎓🏢",

                IntentType.AskSubject => @"
            💬 Chủ đề trọng tâm: Tư vấn về môn học
            - Giới thiệu cấu trúc môn học: 4 môn offline + 1 Coursera online mỗi kỳ.
            - Giải thích rõ các môn có điều kiện tiên quyết và các vấn đề liên quan.
            - Gợi ý phương pháp học tốt từng dạng môn (ví dụ: môn lập trình thì cần thực hành nhiều, môn kỹ năng mềm nên tham gia workshop...). 📚✅",

                _ => @"
            💬 Chủ đề trọng tâm: Hỗ trợ tư vấn chung
            - Sẵn sàng giải đáp bất kỳ câu hỏi nào về ngành học, chương trình đào tạo, combo chuyên ngành, môn học, các thắc mắc học tập khác hoặc tâm sự mọi thứ cùng sinh viên. ✨"
            };

            // 👉 Nếu Intent là Unknown thì không ghép phần commonInfo vào
            var prompt = intent == IntentType.Unknown
                ? $@"{intro}

            {topicSpecificPrompt}

            {guidance}"
                : $@"{intro}

            {commonInfo}

            {topicSpecificPrompt}

            {guidance}";

            return prompt;
        }

        private string BuildPromptGenerateTitle(string userMessage)
        {
            return $@"
Bạn sẽ nhận được một nội dung tin nhắn, nhiệm vụ của bạn:

- Tạo DUY NHẤT 1 tiêu đề ngắn gọn, súc tích, tóm tắt ý chính của tin nhắn đó.
- Tiêu đề phải có độ dài từ 3 đến 5 từ.
- Chỉ trả về nội dung tiêu đề, KHÔNG giải thích, KHÔNG đưa nhiều lựa chọn.
- Tiêu đề cần tự nhiên, dễ hiểu, không chứa ký tự lạ.

Tin nhắn: ""{userMessage}""

Chỉ trả về đúng 1 tiêu đề khoảng 3-5 từ.
";
        }


        public async Task<string> GenerateTiltleAsync(string message)
        {
            var finalPrompt = BuildPromptGenerateTitle(message);

            // Bước 4: Gọi API Gemini (hoặc dịch vụ AI khác)
            var geminiResponse = await geminiApiService.GenerateResponseAsync(finalPrompt);

            // Bước 5: Trả kết quả về cho client
            return geminiResponse;
        }

        private async Task<AcademicContext> BuildAcademicContextObjectAsync()
        {
            var programs = await _programRepository.GetAllDataByExpression(
                filter: c => !c.IsDeleted,
                pageNumber: 1,
                pageSize: int.MaxValue,
                orderBy: null,
                isAscending: true,
                includes: null);

            var majors = await _majorRepository.GetAllDataByExpression(
                filter: c => !c.IsDeleted,
                pageNumber: 1,
                pageSize: int.MaxValue,
                orderBy: m => m.MajorName,
                isAscending: true,
                includes: null);

            var curriculums = await _curriculumRepository.GetAllDataByExpression(
                filter: c => !c.IsDeleted,
                pageNumber: 1,
                pageSize: int.MaxValue,
                orderBy: null,
                isAscending: true,
                includes: c => c.Major);

            var subjects = await _subjectRepository.GetAllDataByExpression(
                filter: c => !c.IsDeleted,
                pageNumber: 1,
                pageSize: int.MaxValue,
                orderBy: null,
                isAscending: true,
                includes: null);

            var subjectInCurriculums = await _subjectInCurriculumRepository.GetAllDataByExpression(
                filter: null,
                pageNumber: 1,
                pageSize: int.MaxValue,
                orderBy: null,
                isAscending: true,
                includes: new Expression<Func<SubjectInCurriculum, object>>[]
                {
                  c => c.Subject,
                  c => c.Curriculum
                });

            var comboSubjects = await _comboSubjectRepository.GetAllDataByExpression(
                filter: null,
                pageNumber: 1,
                pageSize: int.MaxValue,
                orderBy: null,
                isAscending: true,
                includes: new Expression<Func<ComboSubject, object>>[]
                {
                  c => c.Subject,
                  c => c.Combo
                });

            var combos = await _comboRepository.GetAllDataByExpression(
                filter: c => !c.IsDeleted,
                pageNumber: 1,
                pageSize: int.MaxValue,
                orderBy: null,
                isAscending: true,
                includes: c => c.Program);

            var prerequisiteSubjects = await _prerequisiteSubjectService.PrerequisiteExpressionForChat();

            return new AcademicContext
            {
                Majors = majors.Items.Select(m => new
                {
                    m.MajorCode,
                    m.MajorName,
                }).ToList(),

                Curriculums = curriculums.Items.Select(m => new
                {
                    m.CurriculumCode,
                    m.CurriculumName,
                    m.Description,
                    MajorCode = m.Major?.MajorCode ?? string.Empty, // Fix for CS8602  
                }).ToList(),

                Subjects = subjects.Items.Select(m => new
                {
                    m.SubjectCode,
                    m.SubjectName,
                    m.DegreeLevel,
                    m.Description,
                    m.SessionNo,
                    m.SyllabusName,
                    m.MinAvgMarkToPass,
                    m.NoCredit,
                    m.TimeAllocation,
                    m.ScoringScale,
                    m.StudentTasks,
                    m.Note,
                }).ToList(),

                PrerequisiteSubjects = prerequisiteSubjects,

                SubjectInCurriculums = subjectInCurriculums.Items.Select(m => new
                {
                    m.SemesterNo,
                    CurriculumCode = m.Curriculum?.CurriculumCode ?? string.Empty, 
                    SubjectCode = m.Subject?.SubjectCode ?? string.Empty, 
                }).ToList(),

                Programs = programs.Items.Select(m => new
                {
                    m.ProgramCode,
                    m.ProgramName,
                }).ToList(),

                Combos = combos.Items.Select(m => new
                {
                    m.ComboCode,
                    m.ComboName,
                    m.Description,
                    ProgramCode = m.Program?.ProgramCode ?? string.Empty, 
                }).ToList(),

                ComboSubjects = comboSubjects.Items.Select(m => new
                {
                    m.SemesterNo,
                    ComboCode = m.Combo?.ComboCode ?? string.Empty, 
                    SubjectCode = m.Subject?.SubjectCode ?? string.Empty, 
                }).ToList(),
            };
        }

        public async Task<Response> GenerateResponseAsync(Guid? aIChatLogId, string message, TopicChat topicChat)
        {
            Response dto = new Response();
            try
            {
                // Bước 1: Phân tích nội dung tin nhắn để xác định ý định
                var intent = topicChat switch
                {
                    TopicChat.Subject => IntentType.AskSubject,
                    TopicChat.Major => IntentType.AskMajorAdvice,
                    TopicChat.Program => IntentType.AskProgram,
                    TopicChat.Combo => IntentType.AskSpecializationCombo,
                    _ => await intentDetectorService.DetectAsync(message) // TopicChat.Default hoặc các trường hợp khác
                };

                // Bước 2: Thu thập dữ liệu cần thiết từ DB hoặc ngữ cảnh
                string contextData = string.Empty;

                switch (intent)
                {
                    case IntentType.AskMajorAdvice:
                        {
                            var academicContext = await BuildAcademicContextObjectAsync();
                            contextData = JsonSerializerHelper.SerializeData(academicContext);
                            break;
                        }

                    case IntentType.AskSpecializationCombo:
                        {
                            var academicContext = await BuildAcademicContextObjectAsync();
                            contextData = JsonSerializerHelper.SerializeData(academicContext);
                            break;
                        }

                    case IntentType.AskProgram:
                        {
                            var academicContext = await BuildAcademicContextObjectAsync();
                            var pOs = await _programingOutcomeRepository.GetAllDataByExpression(
                                filter: c => !c.IsDeleted,
                                pageNumber: 1,
                                pageSize: int.MaxValue,
                                orderBy: null,
                                isAscending: true,
                                includes: p => p.Program); // Lấy danh sách PO
                            contextData = JsonSerializerHelper.SerializeData(new
                            {
                                academicContext.Majors,
                                academicContext.Curriculums,
                                academicContext.Subjects,
                                academicContext.PrerequisiteSubjects,
                                academicContext.SubjectInCurriculums,
                                academicContext.Programs,
                                academicContext.Combos,
                                academicContext.ComboSubjects,
                                POs = pOs.Items.Select(m => new
                                {
                                    m.ProgramingOutcomeCode,
                                    m.ProgramingOutcomeName,
                                    m.Description,
                                    ProgramCode = m.Program?.ProgramCode ?? string.Empty,
                                }).ToList(),
                            });
                            break;
                        }

                    case IntentType.AskSubject:
                        {
                            var academicContext = await BuildAcademicContextObjectAsync();
                            var tools = await _toolRepository.GetAllDataByExpression(
                                filter: c => !c.IsDeleted,
                                pageNumber: 1,
                                pageSize: int.MaxValue,
                                orderBy: null,
                                isAscending: true,
                                includes: null);
                            var toolForSubjects = await _toolForSubjectRepository.GetAllDataByExpression(
                                filter: null,
                                pageNumber: 1,
                                pageSize: int.MaxValue,
                                orderBy: null,
                                isAscending: true,
                                includes: new Expression<Func<ToolForSubject, object>>[]
                                {
                                    c => c.Subject,
                                    c => c.Tool
                                });
                            var cLOs = await _courseLearningOutcomeRepository.GetAllDataByExpression(
                                filter: c => !c.IsDeleted,
                                pageNumber: 1,
                                pageSize: int.MaxValue,
                                orderBy: null,
                                isAscending: true,
                                includes: c => c.Subject);
                            var assessments = await _assessmentRepository.GetAllDataByExpression(
                                filter: c => !c.IsDeleted,
                                pageNumber: 1,
                                pageSize: int.MaxValue,
                                orderBy: null,
                                isAscending: true,
                                includes: c => c.Subject);
                            var materials = await _materialRepository.GetAllDataByExpression(
                                filter: c => !c.IsDeleted,
                                pageNumber: 1,
                                pageSize: int.MaxValue,
                                orderBy: null,
                                isAscending: true,
                                includes: c => c.Subject);
                            contextData = JsonSerializerHelper.SerializeData(new
                            {
                                academicContext.Majors,
                                academicContext.Curriculums,
                                academicContext.Subjects,
                                academicContext.PrerequisiteSubjects,
                                academicContext.SubjectInCurriculums,
                                academicContext.Programs,
                                academicContext.Combos,
                                academicContext.ComboSubjects,
                                Tools = tools.Items.Select(m => new
                                {
                                    m.ToolCode,
                                    m.ToolName,
                                    m.Description,
                                }).ToList(),
                                ToolForSubjects = toolForSubjects.Items.Select(m => new
                                {
                                    ToolCode = m.Tool?.ToolCode ?? string.Empty,
                                    SubjectCode = m.Subject?.SubjectCode ?? string.Empty,
                                }).ToList(),
                                CLOs = cLOs.Items.Select(m => new
                                {
                                    m.CourseLearningOutcomeCode,
                                    m.CourseLearningOutcomeName,
                                    m.CourseLearningOutcomeDetail,
                                    SubjectCode = m.Subject?.SubjectCode ?? string.Empty,
                                }).ToList(),
                                Assessments = assessments.Items.Select(m => new
                                {
                                    m.Category,
                                    m.Type,
                                    m.Part,
                                    m.Weight,
                                    m.CompletionCriteria,
                                    m.Duration,
                                    m.QuestionType,
                                    m.NoQuestion,
                                    m.KnowledgeAndSkill,
                                    m.GradingGuide,
                                    m.Note,
                                    SubjectCode = m.Subject?.SubjectCode ?? string.Empty,
                                }).ToList(),
                                Materials = materials.Items.Select(m => new
                                {
                                    m.MaterialCode,
                                    m.MaterialName,
                                    m.MaterialDescription,
                                    m.Edition,
                                    m.Note,
                                    m.IsHardCopy,
                                    m.IsOnline,
                                    SubjectCode = m.Subject?.SubjectCode ?? string.Empty,
                                }).ToList(),
                            });
                            break;
                        }
                    default:
                        {
                            contextData = "No Data";
                            break;
                        }
                }
                
                // Bước 3: Tạo prompt gửi đến AI
                var finalPrompt = message;
                if (!string.IsNullOrEmpty(contextData))
                {
                    finalPrompt = BuildPrompt(intent, message, contextData);
                }
                // Thêm lịch sử chat vào prompt nếu có
                if (aIChatLogId != null)
                {
                    var lastMessages = await GetLastMessagesAsync(aIChatLogId.Value, 4);
                    finalPrompt = $@"
                    Lịch sử hội thoại gần đây:
                    {FormatMessageHistory(lastMessages)}
                    {finalPrompt}";
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

        private string FormatMessageHistory(List<Message> messages)
        {
            return string.Join("\n", messages
                .OrderBy(m => m.SentTime)
                .Select(m => $"[{m.SentTime:HH:mm}] {(m.IsBotResponse ? "🤖 Bot" : "🧑‍🎓 User")}: {m.MessageContent}"));
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

        //public async Task<Response> GetAIChatLogActivedByUserId(Guid? userId)
        //{
        //    Response dto = new Response();
        //    try
        //    {
        //        var aIChatLog = await _aIChatLogRepository.GetFirstByExpression(
        //            filter: a => a.UserId == userId && a.IsDeleted == false && a.Status == StatusChat.Actived && a.EndTime == null);
        //        if (aIChatLog == null)
        //        {
        //            dto.IsSucess = true;
        //            dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
        //            dto.Message = "AI chat log not found";
        //            return dto;
        //        }
        //        var message = await _messageRepository.GetAllDataByExpression(
        //            filter: m => m.AIChatLogId == aIChatLog.AIChatLogId,
        //            pageNumber: 1,
        //            pageSize: 20,
        //            orderBy: m => m.SentTime,
        //            isAscending: false,
        //            includes: null);
        //        dto.Data = new
        //        {
        //            AIChatLog = aIChatLog,
        //            Messages = message
        //        };
        //        dto.IsSucess = true;
        //        dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
        //        dto.Message = "AI chat log retrieved successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        dto.IsSucess = false;
        //        dto.BusinessCode = BusinessCode.EXCEPTION;
        //        dto.Message = "An error occurred while retrieving the chat log: " + ex.Message;
        //    }
        //    return dto;
        //}

        private async Task<List<Message>> GetLastMessagesAsync(Guid aIChatLogId, int count = 4)
        {
            // Lấy 4 tin nhắn gần nhất từ cơ sở dữ liệu
            var messages = await _messageRepository.GetAllDataByExpression(
                filter: m => m.AIChatLogId == aIChatLogId,
                pageNumber: 1,
                pageSize: count,
                orderBy: m => m.SentTime,
                isAscending: false, // Lấy tin nhắn gần đây nhất
                includes: null);

            return messages.Items.ToList();
        }

        public async Task<Response> GetAllAIChatLogByTopic(Guid? userId, int pageNumber, int pageSize, bool isDeleted, TopicChat topicChat)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _aIChatLogRepository.GetAllDataByExpression(
                    filter: a => a.UserId == userId
                    && a.IsDeleted == isDeleted
                    && a.Topic == topicChat,
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

        public async Task<Response> UpdateAIChatLog(Guid? userId, Guid aIChatLogId, string? aIChatLogName)
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

                chat.UpdatedAt = DateTime.Now;
                if (!string.IsNullOrEmpty(aIChatLogName)) chat.AIChatLogName = aIChatLogName;

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
                    filter: a => a.UserId == userId
                    && a.AIChatLogId == aIChatLogId
                    && a.IsDeleted == false
                    && a.Status == StatusChat.Actived
                    && a.EndTime == null
                    );

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
                chat.Status = StatusChat.Inactived;

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

        public async Task<Response> CreateAIChatLog(Guid? userId, TopicChat topic)
        {
            Response dto = new Response();
            try
            {
                var chat = new AIChatLog
                {
                    AIChatLogId = Guid.NewGuid(),
                    UserId = userId,
                    Topic = topic,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false,
                    Status = StatusChat.Actived,
                    StartTime = DateTime.Now,
                    EndTime = null,
                    AIChatLogName = "#New Chat " + DateTime.Now.ToString("MMM dd, yyyy HH:mm"),
                    DeletedAt = null,
                    LastMessageTime = DateTime.Now,
                };
                await _aIChatLogRepository.Insert(chat);

                var message = new Message
                {
                    MessageId = Guid.NewGuid(),
                    AIChatLogId = chat.AIChatLogId,
                    SentTime = DateTime.Now,
                    MessageContent = topic switch
                    {
                        TopicChat.Subject => "📚 Xin chào! Mình là chuyên gia tư vấn học thuật của Đại học FPT. Hãy cùng khám phá các môn học trong chương trình và tìm hiểu cách chúng hỗ trợ bạn phát triển kỹ năng và kiến thức chuyên sâu nhé! ✨",

                        TopicChat.Program => "🎓 Chào bạn! Mình sẽ giúp bạn hiểu rõ về lộ trình học và chương trình đào tạo tại Đại học FPT. Cùng nhau khám phá các kỳ học, môn học và cơ hội phát triển toàn diện trong suốt quá trình học tập nhé! 🚀",

                        TopicChat.Major => "🧭 Xin chào! Bạn đang quan tâm đến ngành học phù hợp với sở thích và mục tiêu nghề nghiệp? Mình ở đây để tư vấn và giúp bạn lựa chọn ngành học phù hợp nhất tại Đại học FPT! 🎯",

                        TopicChat.Combo => "🔍 Chào bạn! Mình sẽ giúp bạn hiểu rõ về các combo chuyên ngành tại Đại học FPT, để bạn có thể lựa chọn lộ trình học tập phù hợp với sở thích và định hướng nghề nghiệp của mình! 💼",

                        _ => "👋 Xin chào! Mình là cố vấn học thuật của bạn tại Đại học FPT. Đừng ngần ngại đặt bất kỳ câu hỏi nào bạn còn vướng mắc hoặc trò chuyện vui vẻ cùng mình nhé! 🎉",
                    },
                    IsBotResponse = true,
                    MessageType = MessageType.Text,
                    SenderId = Guid.Empty,
                };
                await _messageRepository.Insert(message);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = new
                {
                    AIChatLog = chat,
                    Message = message
                };
                dto.Message = "AI Chat Log created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating AI Chat Log: " + ex.Message;
            }
            return dto;
        }
    }
}
