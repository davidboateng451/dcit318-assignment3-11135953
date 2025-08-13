using System;
using System.Collections.Generic;
using System.IO;

// ---------- Custom Exceptions ----------
public class InvalidScoreFormatException : Exception
{
    public InvalidScoreFormatException(string message) : base(message) { }
}

public class MissingFieldException : Exception
{
    public MissingFieldException(string message) : base(message) { }
}

// ---------- Student Class ----------
public class Student
{
    public int Id { get; }
    public string FullName { get; }
    public int Score { get; }

    public Student(int id, string fullName, int score)
    {
        Id = id;
        FullName = fullName;
        Score = score;
    }

    public string GetGrade()
    {
        if (Score >= 80 && Score <= 100) return "A";
        if (Score >= 70 && Score <= 79) return "B";
        if (Score >= 60 && Score <= 69) return "C";
        if (Score >= 50 && Score <= 59) return "D";
        return "F";
    }
}

// ---------- StudentResultProcessor ----------
public class StudentResultProcessor
{
    public List<Student> ReadStudentsFromFile(string inputFilePath)
    {
        var students = new List<Student>();

        using (var reader = new StreamReader(inputFilePath))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');

                if (parts.Length != 3)
                {
                    throw new MissingFieldException($"Missing field in line: {line}");
                }

                if (!int.TryParse(parts[0], out int id))
                {
                    throw new FormatException($"Invalid student ID format in line: {line}");
                }

                string fullName = parts[1].Trim();

                if (!int.TryParse(parts[2], out int score))
                {
                    throw new InvalidScoreFormatException($"Invalid score format in line: {line}");
                }

                students.Add(new Student(id, fullName, score));
            }
        }

        return students;
    }

    public void WriteReportToFile(List<Student> students, string outputFilePath)
    {
        using (var writer = new StreamWriter(outputFilePath))
        {
            foreach (var student in students)
            {
                writer.WriteLine($"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {student.GetGrade()}");
            }
        }
    }
}

// ---------- Main Program ----------
class Program
{
    static void Main()
    {
        string inputFilePath = "students.txt";  // Your input file path
        string outputFilePath = "report.txt";   // Output report file path

        try
        {
            var processor = new StudentResultProcessor();

            var students = processor.ReadStudentsFromFile(inputFilePath);

            processor.WriteReportToFile(students, outputFilePath);

            Console.WriteLine($"Report generated successfully at {outputFilePath}");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Error: The input file was not found.");
        }
        catch (InvalidScoreFormatException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (MissingFieldException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
