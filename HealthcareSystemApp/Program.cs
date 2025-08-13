using System;
using System.Collections.Generic;
using System.Linq;

public class Repository<T>
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        items.Add(item);
    }

    public List<T> GetAll()
    {
        return new List<T>(items);
    }

    public T? GetById(Func<T, bool> predicate)
    {
        return items.FirstOrDefault(predicate);
    }

    public bool Remove(Func<T, bool> predicate)
    {
        var item = items.FirstOrDefault(predicate);
        if (item != null)
        {
            items.Remove(item);
            return true;
        }
        return false;
    }
}

public class Patient
{
    public int Id { get; }
    public string Name { get; }
    public int Age { get; }
    public string Gender { get; }

    public Patient(int id, string name, int age, string gender)
    {
        Id = id;
        Name = name;
        Age = age;
        Gender = gender;
    }
}

public class Prescription
{
    public int Id { get; }
    public int PatientId { get; }
    public string MedicationName { get; }
    public DateTime DateIssued { get; }

    public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
    {
        Id = id;
        PatientId = patientId;
        MedicationName = medicationName;
        DateIssued = dateIssued;
    }
}


public class HealthSystemApp
{
    private Repository<Patient> _patientRepo = new Repository<Patient>();
    private Repository<Prescription> _prescriptionRepo = new Repository<Prescription>();
    private Dictionary<int, List<Prescription>> _prescriptionMap = new Dictionary<int, List<Prescription>>();

    public void SeedData()
    {
        _patientRepo.Add(new Patient(1, "Samuel Osei", 30, "Male"));
        _patientRepo.Add(new Patient(2, "Daniella Adjei", 25, "Female"));
        _patientRepo.Add(new Patient(3, "Nelson Ofori", 40, "Male"));

        _prescriptionRepo.Add(new Prescription(1, 1, "Paracetamol", DateTime.Now.AddDays(-10)));
        _prescriptionRepo.Add(new Prescription(2, 1, "Ibuprofen", DateTime.Now.AddDays(-5)));
        _prescriptionRepo.Add(new Prescription(3, 2, "Amoxicillin", DateTime.Now.AddDays(-3)));
        _prescriptionRepo.Add(new Prescription(4, 3, "Metformin", DateTime.Now.AddDays(-1)));
        _prescriptionRepo.Add(new Prescription(5, 3, "Atorvastatin", DateTime.Now));
    }

    public void BuildPrescriptionMap()
    {
        _prescriptionMap.Clear();
        foreach (var prescription in _prescriptionRepo.GetAll())
        {
            if (!_prescriptionMap.ContainsKey(prescription.PatientId))
            {
                _prescriptionMap[prescription.PatientId] = new List<Prescription>();
            }
            _prescriptionMap[prescription.PatientId].Add(prescription);
        }
    }

    public void PrintAllPatients()
    {
        foreach (var patient in _patientRepo.GetAll())
        {
            Console.WriteLine($"ID: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}");
        }
    }

    public List<Prescription> GetPrescriptionsByPatientId(int patientId)
    {
        return _prescriptionMap.ContainsKey(patientId) ? _prescriptionMap[patientId] : new List<Prescription>();
    }

    public void PrintPrescriptionsForPatient(int patientId)
    {
        var prescriptions = GetPrescriptionsByPatientId(patientId);
        if (prescriptions.Count == 0)
        {
            Console.WriteLine("No prescriptions found for this patient.");
        }
        else
        {
            foreach (var prescription in prescriptions)
            {
                Console.WriteLine($"Prescription ID: {prescription.Id}, Medication: {prescription.MedicationName}, Date Issued: {prescription.DateIssued.ToShortDateString()}");
            }
        }
    }
}

public class Program
{
    public static void Main()
    {
        var app = new HealthSystemApp();
        app.SeedData();
        app.BuildPrescriptionMap();

        Console.WriteLine("\n--- All Patients ---");
        app.PrintAllPatients();

        Console.WriteLine("\nEnter Patient ID to view prescriptions: ");
        if (int.TryParse(Console.ReadLine(), out int patientId))
        {
            Console.WriteLine($"\n--- Prescriptions for Patient ID {patientId} ---");
            app.PrintPrescriptionsForPatient(patientId);
        }
        else
        {
            Console.WriteLine("Invalid Patient ID.");
        }
    }
}
