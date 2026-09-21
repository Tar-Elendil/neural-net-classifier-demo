using ML.NET.API.Models;
using ML_NET_Training;

namespace ML.NET.API.Services;

public interface IPredictClassifications
{
    bool CheckIfFraud(CardTransaction cardTransaction);
}

public class PredictionService : IPredictClassifications
{
    public bool CheckIfFraud(CardTransaction cardTransaction)
    {
        //Load sample data
        var sampleData = new MLModel1.ModelInput()
        {
            User = 0F,
            Card = 0F,
            Year = 2002F,
            Month = 9F,
            Day = 1F,
            Time = @"06:42",
            Use_Chip = @"Swipe Transaction",
            Merchant_Name = -7.2761206E+17F,
            Merchant_City = @"Monterey Park",
            Merchant_State = @"CA",
            MCC = 5411F,
            Errors_ = @"",
        };

        //Load model and predict output
        var result = MLModel1.Predict(sampleData);
        return result.PredictedLabel == "Yes";
    }
}

