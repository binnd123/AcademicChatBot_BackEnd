using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.Enum
{
    public enum IntentType
    {
        Unknown = 0,
        AskMajorAdvice = 1,
        AskSpecializationCombo = 2,
        AskProgram = 3,
        AskSubject = 4

        //// --- Tư vấn môn học ---
        //AskSubjectDetails,          // Hỏi chi tiết về môn học cụ thể
        //AskPrerequisiteSubjects,    // Hỏi về các môn tiên quyết
        //AskElectiveRecommendations, // Gợi ý các môn tự chọn phù hợp
        //AskSemesterSubjects,        // Hỏi các môn học trong một học kỳ cụ thể

        //// --- Quản lý điểm và GPA ---
        //AskGPAAdvice,               // Hỏi cách cải thiện GPA
        //AskRetakePolicy,            // Hỏi về chính sách học lại/học cải thiện
        //AskGradeCalculation,        // Hỏi cách tính điểm tổng kết / GPA

        //// --- Thủ tục hành chính ---
        //AskEnrollmentProcedure,     // Hỏi thủ tục đăng ký môn
        //AskInternshipProcedure,     // Hỏi quy trình đăng ký thực tập
        //AskGraduationRequirements,  // Hỏi điều kiện tốt nghiệp
        //AskDocumentRequest,         // Xin giấy xác nhận, bảng điểm, v.v.

        //// --- Định hướng nghề nghiệp ---
        //AskCareerAdvice,            // Hỏi định hướng nghề nghiệp sau tốt nghiệp
        //AskSkillDevelopment,        // Hỏi kỹ năng cần thiết theo ngành
        //AskCertificationAdvice,     // Hỏi chứng chỉ nên học thêm (ngoài Coursera)

        //// --- Thông tin sự kiện ---
        //AskUpcomingEvents,          // Hỏi về các sự kiện học tập sắp tới
        //AskCompetitionInfo,         // Hỏi thông tin về các cuộc thi, hackathon

        //// --- Hệ thống ---
        //AskBotUsageHelp,            // Hỏi cách sử dụng chatbot
        //GiveFeedback,               // Đưa phản hồi cho hệ thống
    }

}
