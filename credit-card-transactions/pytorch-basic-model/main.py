import helpers
import pandas as pd # pyright: ignore[reportMissingModuleSource]
import torch # pyright: ignore[reportMissingImports]
import torch.nn as nn # pyright: ignore[reportMissingImports]
import sys
from datetime import datetime
from simple_model import SimpleClassifier
from binary_classifier import BinaryClassifier

__device__ = torch.device("cuda" if torch.cuda.is_available() else "cpu")

def convert_time(time: str) -> int:
    try:
        split = time.split(":")
        return int(split[0]) + (24*int(split[1]))
    except:
        return 0

def convert_amount(amount: str) -> float:
    try:
        return float(amount[1:])
    except:
        return 0

def convert_IsFraud(is_fraud: str) -> float:
    return 1 if is_fraud == 'Yes' else 0

def main():
    if not helpers.file_exists("card_transaction.v1.csv"):
        print(f"CSV file {'card_transaction.v1.csv'} not found")
        sys.exit(1)

    # read data
    df = pd.read_csv('card_transaction.v1.csv')
    df = df.drop(columns = ['Merchant Name', 'Merchant City', 'Merchant State', 'Errors?'])
    size = len(df)
    train_size = int(size*0.8)
    test_size = size-train_size

    # convert to numerical data
    df['Time']=df['Time'].map(convert_time)
    df['Amount']=df['Amount'].map(convert_amount)
    df['Is Fraud?']=df['Is Fraud?'].map(convert_IsFraud)

    # fill NaN values
    df.fillna({'Zip': 0}, inplace=True)

    categorical_variables = ['Use Chip']
    numerics = ['User', 'Card', 'Year', 'Month', 'Day', 'Zip', 'MCC']
    
    # Encode the categorical variables
    df = pd.get_dummies(df, columns = categorical_variables, dtype=float)

    # Split into training and test datasets
    test_df = df.sample(frac=0.2, random_state=42)
    train_df = df.drop(test_df.index)

    # split the output column
    train_output_df = train_df['Is Fraud?']
    test_output_df = test_df['Is Fraud?']
    train_df = train_df.drop(columns=['Is Fraud?'])
    test_df = test_df.drop(columns=['Is Fraud?'])

    # Normalise numerical data 
    means = train_df[numerics].mean()
    sd = train_df[numerics].std()
    train_df[numerics]= (train_df[numerics] - means)/sd
    test_df[numerics]= (test_df[numerics] - means)/sd

    # Convert to tensors
    training_set = torch.from_numpy(train_df.values).float()
    test_data = torch.from_numpy(test_df.values).float()
    training_outputs = torch.from_numpy(train_output_df.values).unsqueeze(1).float()
    test_outputs = torch.from_numpy(test_output_df.values).unsqueeze(1).float()

    # Training loop
    # Loop over epochs and batches.
    # Compute the forward pass, calculate loss, and perform backpropagation.
    model = SimpleClassifier(num_features=12, num_hidden=32)
    criterion = nn.BCEWithLogitsLoss()
    optimizer = torch.optim.Adam(model.parameters(), lr=0.001)

    for epoch in range(50):
        model.train()                            # Set model to training mode
        optimizer.zero_grad()                    # Clear previous gradients 
        outputs = model(training_set)                # Forward pass 
        loss = criterion(outputs, training_outputs)      # Calculate loss 
        loss.backward()                           # Backward pass to compute gradients 
        optimizer.step()                         # Update weights 

        # Eval model
        model.eval()
        with torch.no_grad():
            predictions = model(test_data)
            pred_loss = criterion(predictions, test_outputs)
        model.train()
        print(f"Training Accuracy: {loss:.2f}")
        print(f"Test Accuracy: {pred_loss:.2f}")


if __name__ == "__main__":
    main()