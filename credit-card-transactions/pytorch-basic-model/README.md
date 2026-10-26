# Overview

## Dataset
This classifier is trained on the open source dataset available at this [location](https://github.com/IBM/TabFormer).
This is a synthetic dataset and the unzipped csv file is ~2GB

## Defining the model
13 inputs
### Create Hidden Layers
- Add layers for categorical data. 
- Use 1 level of hidden layers for simplicity
- Expect a single True|False output value ie. whether the transaction is fraudulent or not


### Define Activation function
use Relu

## Training the model
get the weights + biases

### Algorithm

- Read input
- Drop columns not needed for training
- Encode categorical values into numerical
- Normalise data to reduce variance
- Define loss function
- Make predictions
- Run epoch
- Compare weights against test data
- If validation error begins to increase. Stop
