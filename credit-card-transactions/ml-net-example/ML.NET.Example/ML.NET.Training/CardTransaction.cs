using Microsoft.ML.Data;

namespace ML.NET.Training;

public class CardTransaction
{
    [LoadColumn(0)]
    public string User { get; set; } = string.Empty;
    [LoadColumn(1)]
    public long Card { get; set; }
    [LoadColumn(2)]
    public int Year { get; set; }
    [LoadColumn(3)]
    public int Month { get; set; }
    [LoadColumn(4)]
    public int Day { get; set; }
    [LoadColumn(5)]
    public DateTime Time { get; set; }
    [LoadColumn(6)]
    public string UseChip { get; set; } = string.Empty;
    [LoadColumn(7)]
    public string MerchantName { get; set; } = string.Empty;
    [LoadColumn(8)]
    public string MerchantCity { get; set; } = string.Empty;
    [LoadColumn(9)]
    public string MerchantState { get; set; } = string.Empty;
    [LoadColumn(10)]
    public double Zip { get; set; }
    [LoadColumn(11)]
    public int MCC { get; set; }
    [LoadColumn(12)]
    public string Errors { get; set; } = string.Empty;
    [LoadColumn(13)]
    public bool IsFraud { get; set; }
}