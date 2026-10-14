import helpers
import pandas as pd # pyright: ignore[reportMissingModuleSource]
import torch # pyright: ignore[reportMissingImports]
import torch.nn as nn # pyright: ignore[reportMissingImports]
import sys
from datetime import datetime
from simple_model import SimpleClassifier

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

def check_accuracy(loader, model: nn.Module):
    num_correct = 0
    num_samples = 0
    model.eval()
    with torch.no_grad():
        for x, y in loader:
            x = x.to(__device__).reshape(x.shape[0], -1)
            y = y.to(__device__)
            scores = model(x)
            _, predictions = scores.max(1)
            num_correct += (predictions == y).sum()
            num_samples += predictions.size(0)
    model.train()
    return num_correct / num_samples

def main():
    if not helpers.file_exists("card_transaction.v1.csv"):
        print(f"CSV file {'card_transaction.v1.csv'} not found")
        sys.exit(1)

    # read data
    df = pd.read_csv('card_transaction.v1.csv')
    df = df.drop(columns = ['Merchant Name', 'Merchant City', 'Merchant State', 'Errors?'])

    # convert to numerical data
    df['Time']=df['Time'].map(convert_time)
    df['Amount']=df['Amount'].map(convert_amount)

    categorical_variables = ['Use Chip', 'Is Fraud?']
    numerics = ['User', 'Card', 'Year', 'Month', 'Day', 'Zip', 'MCC']
    
    # One Hot encode the categorical variables
    df = pd.get_dummies(df, columns = categorical_variables, dtype=float)

    # Split into training and test datasets
    test_df = df.sample(frac=0.2, random_state=42)
    train_df = df.drop(test_df.index)

    # Normalise numerical data 
    means = train_df[numerics].mean()
    sd = train_df[numerics].std()
    train_df[numerics]= (train_df[numerics] - means)/sd
    test_df[numerics]= (test_df[numerics] - means)/sd

    # Convert to tensor
    training_set = torch.from_numpy(train_df.values).float()
    test_data = torch.from_numpy(test_df.values).float()

    # Training loop
    # Loop over epochs and batches.
    # Compute the forward pass, calculate loss, and perform backpropagation.
    model = SimpleClassifier(num_features=14, num_hidden=32)
    criterion = nn.MSELoss()
    optimizer = torch.optim.SGD(model.parameters(), lr=0.001)
    model.train() # Set model to training mode

    for epoch in range(5):                       # Training for 5 epochs 
        optimizer.zero_grad()                    # Clear previous gradients 
        outputs = model(training_set)                # Forward pass 
        loss = criterion(outputs, training_set)      # Calculate loss 
        loss.backward()                           # Backward pass to compute gradients 
        optimizer.step()                         # Update weights 
        print(f'Epoch [{epoch + 1}/5], Loss: {loss.item():.4f}') 

    # Eval model
    train_acc = check_accuracy(training_set, model)
    test_acc = check_accuracy(test_data, model)
    print(f"Training Accuracy: {train_acc:.2f}")
    print(f"Test Accuracy: {test_acc:.2f}")

if __name__ == "__main__":
    main()