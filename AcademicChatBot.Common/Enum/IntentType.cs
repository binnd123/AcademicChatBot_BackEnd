using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.Enum
{
    public enum IntentType
    {
        Unknown,                // Khi không xác định được ý định

        // --- Tư vấn học tập ---
        AskMajorAdvice,             // Xin tư vấn chọn ngành học phù hợp
        AskSpecializationCombo,     // Xin gợi ý chọn tổ hợp chuyên ngành (combo) phù hợp từ kỳ 5
        AskStudyPlan,               // Xin gợi ý lộ trình học tập (theo từng kỳ hoặc mục tiêu)
        AskElectiveSubjects,        // Xin gợi ý môn học tự chọn phù hợp
        AskCapstoneAdvice,          // Xin gợi ý về đồ án/capstone project
        CompareSemesterProgress,    // So sánh tiến độ giữa các học kỳ (ví dụ: kỳ 5 vs kỳ 6)
        AskInternshipAdvice,        // Xin lời khuyên về kỳ thực tập
        AskCourseraStrategy,        // Hỏi cách học Coursera hiệu quả (vì mỗi kỳ đều có)
    }
}
