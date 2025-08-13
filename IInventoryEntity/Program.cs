using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public interface IInventoryEntity
{
    int Id { get; }
}

public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

public class InventoryLogger<T> where T : IInventoryEntity
{
    private List<T> _log = new List<T>();
    private readonly string _filePath;

    public InventoryLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Add(T item)
    {
        _log.Add(item);
        Console.WriteLine($"Added: {item}");
    }

    public List<T> GetAll()
    {
        return _log;
    }

    public void SaveToFile()
    {
        try
        {
            string json = JsonSerializer.Serialize(_log, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
            Console.WriteLine($"Data saved to {_filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving to file: {ex.Message}");
        }
    }

    public void LoadFromFile()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("No saved data found.");
                return;
            }

            string json = File.ReadAllText(_filePath);
            var items = JsonSerializer.Deserialize<List<T>>(json);
            if (items != null)
            {
                _log = items;
                Console.WriteLine("Data loaded successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading from file: {ex.Message}");
        }
    }
}

public class InventoryApp
{
    private InventoryLogger<InventoryItem> _logger;

    public InventoryApp(string filePath)
    {
        _logger = new InventoryLogger<InventoryItem>(filePath);
    }

    public void SeedSampleData()
    {
        _logger.Add(new InventoryItem(1, "Laptop", 5, DateTime.Now));
        _logger.Add(new InventoryItem(2, "Mouse", 15, DateTime.Now));
        _logger.Add(new InventoryItem(3, "Keyboard", 8, DateTime.Now));
        _logger.Add(new InventoryItem(4, "Monitor", 3, DateTime.Now));
        _logger.Add(new InventoryItem(5, "Printer", 2, DateTime.Now));
    }

    public void SaveData()
    {
        _logger.SaveToFile();
    }

    public void LoadData()
    {
        _logger.LoadFromFile();
    }

    public void PrintAllItems()
    {
        var items = _logger.GetAll();
        if (items.Count == 0)
        {
            Console.WriteLine("No items to display.");
            return;
        }

        foreach (var item in items)
        {
            Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Qty: {item.Quantity}, Added: {item.DateAdded}");
        }
    }
}

class Program
{
    static void Main()
    {
        string filePath = "inventory.json";

        var app = new InventoryApp(filePath);

        app.SeedSampleData();
        app.SaveData();

        Console.WriteLine("\n--- New Session ---");
        var newApp = new InventoryApp(filePath);
        newApp.LoadData();
        newApp.PrintAllItems();
    }
}
