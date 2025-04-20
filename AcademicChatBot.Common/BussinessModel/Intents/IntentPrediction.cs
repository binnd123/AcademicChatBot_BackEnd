using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace AcademicChatBot.Common.BussinessModel.Intents
{
    public class IntentPrediction
    {
        [ColumnName("PredictedLabel")]
        public int PredictedIntent { get; set; }
    }
}
