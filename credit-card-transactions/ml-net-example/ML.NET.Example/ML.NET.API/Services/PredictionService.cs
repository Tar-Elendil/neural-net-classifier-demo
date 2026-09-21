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
        var data = new MLModel1.ModelInput()
        {
            User = cardTransaction.User,
            Card = cardTransaction.Card,
            Amount = $"${cardTransaction.Amount}",
            Year = cardTransaction.Year,
            Month = cardTransaction.Month,
            Day = cardTransaction.Day,
            Time = cardTransaction.Time.ToString("HH:mm"),
            Use_Chip = cardTransaction.UseChip,
            Merchant_Name = long.Parse(cardTransaction.MerchantName),
            Merchant_City = cardTransaction.MerchantCity,
            Merchant_State = cardTransaction.MerchantState,
            MCC = cardTransaction.MCC,
        };

        //Load model and predict output
        var result = MLModel1.Predict(data);
        return result.PredictedLabel == "Yes";
    }
}

