import torch # pyright: ignore[reportMissingImports]
import torch.nn as nn # pyright: ignore[reportMissingImports]

class BinaryClassifier(nn.Module):
    def __init__(self, in_features, hidden_size=16):
        super().__init__()
        self.net = nn.Sequential(
            nn.Linear(in_features, hidden_size),
            nn.ReLU(),
            nn.Linear(hidden_size, 1),   # one output: the logit
        )

    def forward(self, x):
        return self.net(x).squeeze(-1)   # shape (batch,)