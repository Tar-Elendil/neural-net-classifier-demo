import torch.nn as nn # pyright: ignore[reportMissingImports]

class SimpleClassifier(nn.Module):
    def __init__(self, num_features: int, num_hidden: int) -> None:
        super(SimpleClassifier, self).__init__()
        # Hidden layers
        self.hidden = nn.Linear(in_features=num_features, out_features=num_hidden, bias=True)

        # Output layer
        self.output = nn.Linear(in_features=num_hidden, out_features=1, bias=True)
        
        self.activation = nn.ReLU()
    
    def forward(self, x):
        x = self.activation(self.hidden(x))
        x = self.output(x)
        return x