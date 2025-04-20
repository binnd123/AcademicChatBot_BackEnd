using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class IntentDetectorService : IIntentDetectorService
    {
        private readonly IClassificationService _classificationService;

        public IntentDetectorService(IClassificationService classificationService)
        {
            _classificationService = classificationService;
        }

        public async Task<IntentType> DetectAsync(string message)
        {
            // Sử dụng ClassificationIntent để lấy dự đoán từ mô hình intent
            var intentPrediction = await _classificationService.ClassificationIntent(message);
            return MapPredictionToIntentType(intentPrediction);
        }
        private IntentType MapPredictionToIntentType(int predictedIntent)
        {
            if (Enum.IsDefined(typeof(IntentType), predictedIntent))
            {
                return (IntentType)predictedIntent;
            }
            else
            {
                return IntentType.Unknown;
            }
        }
    }
}
