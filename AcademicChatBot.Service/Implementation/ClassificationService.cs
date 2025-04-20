using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel.Intents;
using AcademicChatBot.Service.Contract;
using Microsoft.ML;

namespace AcademicChatBot.Service.Implementation
{
    public class ClassificationService : IClassificationService
    {
        private readonly PredictionEngine<IntentData, IntentPrediction> _intentPredictionEngine;

        public ClassificationService(PredictionEngine<IntentData, IntentPrediction> intentPredictionEngine)
        {
            _intentPredictionEngine = intentPredictionEngine;
        }

        public async Task<int> ClassificationIntent(string message)
        {
            var intentData = new IntentData
            {
                Message = message
            };

            var prediction = _intentPredictionEngine.Predict(intentData);
            Console.WriteLine($"Intent Prediction: {prediction.PredictedIntent}");

            return prediction.PredictedIntent;
        }
    }
}
