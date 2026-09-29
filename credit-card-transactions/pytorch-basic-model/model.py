import torch.nn as nn

class TransactionsModel(nn.Module):
    def __int__(self):
        super(TransactionsModel, self).__init__()
        # Hidden layers
        self.hidden = nn.Linear(in_features=13, out_features=32, bias=True)

        # Output layer
        self.output = nn.Linear(in_features=32, out_features=1, bias=True)
        
        self.activation = nn.ReLU()
    
    def forward(self, x):
        x = self.activation(self.hidden(x))
        x = self.output(x)
        return x