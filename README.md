# dcit318-assignment3-11135953
## 1. FinanceManagementSystem
A C# console application designed to manage and track personal or business financial transactions.

**Features:**
- Records income and expenses.
- Categorizes transactions.
- Calculates total balance.
- Supports saving and loading data from a file.
- Implements error handling for invalid input.

---

## 2. HealthcareSystemApp
A healthcare management console application for recording and retrieving patient data.

**Features:**
- Stores patient personal and medical information.
- Allows searching for patients by ID or name.
- Supports updating patient records.
- Saves and loads patient data using file operations.
- Includes proper exception handling for missing or corrupted data.

---

## 3. IInventoryEntity
A **marker interface** used in inventory-related applications.

**Purpose:**
- Declares the `Id` property for all inventory entities.
- Enforces a consistent structure for identifying inventory records.
- Used as a type constraint in generic inventory classes (e.g., `InventoryLogger<T>`).

---

## 4. IInventoryItem
A C# **interface** representing inventory items.

**Properties:**
- `int Id` – Unique identifier for the item.
- `string Name` – Name of the item.
- `int Quantity` – Quantity in stock.

**Usage:**
- Implemented by product classes like `ElectronicItem` and `GroceryItem`.
- Ensures consistency across all inventory items.

---

## 5. StudentGradingSystem
A C# application for reading student data from a `.txt` file, validating the input, assigning grades, and writing a summary report.

**Features:**
- Reads `ID, FullName, Score` from an input file.
- Validates data and throws custom exceptions:
  - `InvalidScoreFormatException`
  - `MissingFieldException`
- Assigns grades:
  - 80–100 → `A`
  - 70–79 → `B`
  - 60–69 → `C`
  - 50–59 → `D`
  - Below 50 → `F`
- Writes a clean report to an output file.
- Handles file and format errors gracefully.
