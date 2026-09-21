# Overview

This demo is based on the Microsoft ML.NET demo on [Sentiment Analysis](https://github.com/dotnet/machinelearning-samples/tree/main/samples/csharp/getting-started/BinaryClassification_SentimentAnalysis)

## Dataset
This classifier is trained on the open source dataset available at this [location](https://github.com/IBM/TabFormer).
This is a synthetic dataset and the unzipped csv file is ~2GB

## Environment
The following will be needed
- Visual Studio 2026
- Docker
- Access to the public nuget.org and DockerHub repoistories

## ML.NET
ML.NET Model Builder in Visual Studio can be used to train a model with the given dataset.
See the full [ML.NET  tutorial](https://dotnet.microsoft.com/en-us/learn/ml-dotnet/get-started-tutorial/intro) on the Microsoft docs site.<br>
This will generate the training code, however it is possible to write this training code as a standalone application that may be used to retrain the model.

### Setup
The model config can be found in the [MLModel1.mbconfig](ml-net-example/ML.NET.Example//ML.NET.Training/MLModel1.mbconfig) file. This is read by Visual Studio and will contain
- Training data setup
- Model type and accuracy

The actual ML model is contained in the file [MLModel1.mlnet](ml-net-example/ML.NET.Example/ML.NET.Training/MLModel1.mlnet) <br>
This file is around ~1MB in size and ~73% accurate in predicting the `Is Fraud?` label in the training data. A more accurate model can be generated however this will result in a larger file size.

## Running locally
As the ML model is wholly contained in the repo, a simple build command in the [ML.NET.Example](ml-net-example/ML.NET.Example/) folder
```{docker}
docker build -t ml-net-example .
```

will generate the image locally which may be run with the command
```{docker}
docker run -d -p 8080:8080 ml-net-example
```