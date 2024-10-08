# ExamScoreCalculator

## Description

ExamScoreCalculator is a C# console application designed to calculate and process exam scores for multiple students across different faculties. The program reads student answers from text files, compares them with answer keys, calculates scores, and outputs the results to a CSV file.

## Features

- Reads answer keys from a specified file
- Processes multiple student exam files
- Calculates scores based on different booklet types
- Handles errors and writes them to a separate file
- Outputs results in CSV format

## How to Use

1. Prepare the answer key file and place it in the `./Files/cevap/` directory.
2. Place student exam files in the `./Files/sinav/` directory.
3. Run the program and follow the prompts (note: in the current version, file paths are hardcoded).
4. Results will be saved in `./Files/cevap/Sonuc.csv`.
5. Any errors encountered will be logged in `./Files/cevap/HatalıOkunanlar.csv`.

## Code Breakdown

### Main Method

1. Sets up directory paths for input and output files.
2. Calls `ReadStudentAnswerWithOneFile()` to process exam files.
3. Handles exceptions and waits for user input before closing.

### ReadStudentAnswerWithOneFile Method

1. Reads all `.txt` files in the exams directory.
2. Processes each file using `ReadStudentAnswersByFaculty()`.
3. Writes results to `Sonuc.csv` and errors to `HatalıOkunanlar.csv`.

### ReadStudentAnswersByFaculty Method

1. Reads lines from a single exam file.
2. Parses student ID, booklet type, and answers.
3. Calculates scores using `CalculateStudentScore()`.
4. Formats results for output.

### CalculateStudentScore Method

1. Compares student answers with the answer key.
2. Handles different booklet types.
3. Returns an array of scores for different sections.

### ReadAnswerKeys Method

1. Reads the answer key file.
2. Parses answer keys for different booklet types.
3. Returns a dictionary of answer keys.

## Error Handling

The program includes error checking for:
- Invalid student IDs
- Incomplete answer data
- File reading/writing errors

Errors are logged in the `HatalıOkunanlar.csv` file.

## Notes

- The current version has some hardcoded file paths and doesn't use user input for file locations.
- The program is designed to handle multiple booklet types and can determine the correct type if not specified.
