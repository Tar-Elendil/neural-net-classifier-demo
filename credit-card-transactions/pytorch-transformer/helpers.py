import csv
from typing import Generator
from pathlib import Path


def file_exists(file_path: str) -> bool:
    return Path(file_path).is_file()

def yes_or_no(input: str) -> bool:
    return input.lower() == "yes"

def read_csv(file_path: str) -> Generator[str]:
    with open(file_path, mode='r', newline='', encoding='utf-8') as file:
        reader = csv.reader(file, delimiter=',', quotechar="'")
        i = 0
        for row in reader:
            i = i+1
            yield row
            if i > 10: break
