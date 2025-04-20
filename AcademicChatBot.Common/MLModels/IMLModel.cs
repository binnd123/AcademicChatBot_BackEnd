using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;

namespace AcademicChatBot.Common.MLModels
{
    public interface IMLModel
    {
        MLContext MlContext { get; }
        ITransformer TransactionModel { get; }
        ITransformer IntentModel { get; }
    }
}
