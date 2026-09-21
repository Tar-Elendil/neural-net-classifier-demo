namespace ML.NET.API.Models;

public class CardTransaction
{
    public uint User { get; set; } 
    
    public long Card { get; set; }
    public decimal Amount { get; set; }
    
    public int Year { get; set; }
    
    public int Month { get; set; }
    
    public int Day { get; set; }
    
    public TimeOnly Time { get; set; }
    
    public string UseChip { get; set; } = string.Empty;
    
    public string MerchantName { get; set; } = string.Empty;
    
    public string MerchantCity { get; set; } = string.Empty;
    
    public string MerchantState { get; set; } = string.Empty;
    
    public double Zip { get; set; }
    
    public int MCC { get; set; }
}