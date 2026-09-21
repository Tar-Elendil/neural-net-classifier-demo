import helpers
import pandas as pd # pyright: ignore[reportMissingModuleSource]
import sys

def main():
    if not helpers.file_exists("card_transaction.v1.csv"):
        print(f"CSV file {'card_transaction.v1.csv'} not found")
        sys.exit(1)

    df = pd.read_csv('card_transaction.v1.csv')

if __name__ == "__main__":
    main()