using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace AcademicChatBot.Common.BussinessModel.Intents
{
    public class IntentData
    {
        [LoadColumn(0)]
        public string Message { get; set; }

        [LoadColumn(1)]
        [ColumnName("Label")]
        public int Intent { get; set; }
    }
}
