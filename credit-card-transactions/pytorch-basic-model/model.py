import torch.nn as nn

class TransactionsModel(nn.Module):
    def __int__(self):
        super().__init__()
        # Hidden layers
        self.hidden = nn.Linear(in_features=13, out_features=32, bias=True)

        # Hidden layer 1
        self.output = nn.Linear(in_features=32, out_features=1, bias=True)
        
        self.activation = nn.ReLU()
