using Bogus;
using ML.NET.API.Models;

namespace ML.NET.API.Tests;

public static class TestData
{
    private static readonly Faker Faker = new();

    private static readonly string[] TransactionTypes = ["Swipe Transaction", "Online Transaction", "Chip Transaction"];
    public static CardTransaction CardTransaction() =>
        new() 
        {
            User = (uint)Random.Shared.Next(0, 2000),
            UseChip = Faker.PickRandom(TransactionTypes),
            Card = Random.Shared.Next(0, 9),
            Year = Random.Shared.Next(1991, 2026),
            Month = Random.Shared.Next(1,13),
            Day = Random.Shared.Next(1, 28),
            Time = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(Random.Shared.Next(12))),
            MerchantCity = Faker.Address.City(),
            MerchantName = Random.Shared.NextInt64().ToString(),
            MerchantState = Faker.Address.State(),
            Zip = Random.Shared.Next(),
            MCC = Random.Shared.Next(1000, 9999),
        };
}