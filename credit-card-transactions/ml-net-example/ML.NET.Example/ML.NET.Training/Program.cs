using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
using static Microsoft.ML.Transforms.Text.TextNormalizingEstimator;

namespace ML.NET.Training;

class Program
{
    private static void SaveModel(MLContext mlContext, ITransformer model, string modelFilePath, DataViewSchema trainingDataSchema)
    {
        mlContext.Model.Save(model, trainingDataSchema, modelFilePath);

        Console.WriteLine("Saved model to " + modelFilePath);
    }

    static int Main(string[] args)
    {
        // Validate file
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a dataset for training");
            return 1;
        }

        var filePath = args[0];
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Unable to read file");
            return 1;
        }

        //Step 1. Create an ML Context
        var ctx = new MLContext();

        //Step 2. Read in the input data from a text file for model training
        IDataView trainingData = ctx.Data
            .LoadFromTextFile<CardTransaction>(filePath, hasHeader: true);

        //Get all the feature column names (All except the Label and the IdPreservationColumn)
        string[] stringColumnNames = [nameof(CardTransaction.User), nameof(CardTransaction.UseChip), nameof(CardTransaction.MerchantName), 
            nameof(CardTransaction.MerchantCity), nameof(CardTransaction.MerchantState)];
        string[] featureColumnNames = [.. trainingData.Schema.AsQueryable() 
            // Get alll the column names
            .Select(column => column.Name)                               
            .Where(name => name != nameof(CardTransaction.IsFraud))];

        //Step 3. Build your data processing and training pipeline
        var pipeline = ctx.Transforms.Concatenate("StringFeatures", stringColumnNames)
            .Append(ctx.Transforms.NormalizeMeanVariance(inputColumnName: "StringFeatures", outputColumnName: "StringFeaturesNormalizedByMeanVar"));

        // Set the training algorithm
        var trainer = ctx.BinaryClassification.Trainers
            .LbfgsLogisticRegression(labelColumnName: nameof(CardTransaction.IsFraud), featureColumnName: "StringFeaturesNormalizedByMeanVar");

        //Step 4. Train your model
        ITransformer trainedModel = pipeline.Append(trainer).Fit(trainingData);


        // Save model for use by consuming services
        SaveModel(ctx, trainedModel, "credit_fraud_model", trainingData.Schema);

        return 0;
    }
};