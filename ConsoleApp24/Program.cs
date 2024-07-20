using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsoleApp24;

namespace ConsoleApp22
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                List<Patient> patients = LoadPatients();
                List<Doctor> doctors = new List<Doctor>
                {
                    new Doctor { FirstName = "Ehmed", LastName = "Seyidov", YearsOfExperience = 10, AppointmentSlots = new List<bool> { true, true, true } },
                    new Doctor { FirstName = "Gulendam", LastName = "Bagirli", YearsOfExperience = 5, AppointmentSlots = new List<bool> { true, true, true } },
                    new Doctor { FirstName = "Bob", LastName = "Rzayev", YearsOfExperience = 8, AppointmentSlots = new List<bool> { true, true, true } },
                    new Doctor { FirstName = "Ehmed", LastName = "Muradov", YearsOfExperience = 12, AppointmentSlots = new List<bool> { true, true, true } },
                    new Doctor { FirstName = "Tom", LastName = "Shelby", YearsOfExperience = 7, AppointmentSlots = new List<bool> { true, true, true } },
                    new Doctor { FirstName = "Sara", LastName = "Bunyadova", YearsOfExperience = 9, AppointmentSlots = new List<bool> { true, true, true } },
                    new Doctor { FirstName = "Mehmet", LastName = "Karahanli", YearsOfExperience = 6, AppointmentSlots = new List<bool> { true, true, true } },
                    new Doctor { FirstName = "Famil", LastName = "Davudov", YearsOfExperience = 11, AppointmentSlots = new List<bool> { true, true, true } },
                    new Doctor { FirstName = "Ali", LastName = "Candan", YearsOfExperience = 8, AppointmentSlots = new List<bool> { true, true, true } }
                };

                while (true)
                {
                    try
                    {
                        Console.Write("Enter your first name: ");
                        string firstName = Console.ReadLine();
                        Console.Write("Enter your last name: ");
                        string lastName = Console.ReadLine();
                        Console.Write("Enter your email: ");
                        string email = Console.ReadLine();
                        Console.Write("Enter your phone number: ");
                        string phone = Console.ReadLine();

                        Console.WriteLine("\nDepartments:");
                        Console.WriteLine("1. Pediatrics");
                        Console.WriteLine("2. Traumatology");
                        Console.WriteLine("3. Stomatology");

                        Console.Write("\nEnter your department choice (1-3): ");
                        if (!int.TryParse(Console.ReadLine(), out int departmentChoice) || departmentChoice < 1 || departmentChoice > 3)
                        {
                            Console.WriteLine("Invalid department choice. Please enter a number between 1 and 3.");
                            continue;
                        }

                        List<Doctor> departmentDoctors = new List<Doctor>();
                        switch (departmentChoice)
                        {
                            case 1:
                                departmentDoctors.AddRange(doctors.GetRange(0, 3));
                                break;
                            case 2:
                                departmentDoctors.AddRange(doctors.GetRange(3, 2));
                                break;
                            case 3:
                                departmentDoctors.AddRange(doctors.GetRange(5, 4));
                                break;
                        }

                        Console.WriteLine("\nDoctors:");
                        for (int i = 0; i < departmentDoctors.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {departmentDoctors[i].FirstName} {departmentDoctors[i].LastName} ({departmentDoctors[i].YearsOfExperience} years of experience)");
                        }

                        Console.Write("\nEnter your doctor choice (1-{0}): ", departmentDoctors.Count);
                        if (!int.TryParse(Console.ReadLine(), out int doctorChoice) || doctorChoice < 1 || doctorChoice > departmentDoctors.Count)
                        {
                            Console.WriteLine($"Invalid doctor choice. Please enter a number between 1 and {departmentDoctors.Count}.");
                            continue;
                        }
                        Doctor selectedDoctor = departmentDoctors[doctorChoice - 1];

                        Console.WriteLine("\nAppointment Slots:");
                        Console.WriteLine("1. 09:00-11:00 " + (selectedDoctor.AppointmentSlots[0] ? "Available" : "Booked"));
                        Console.WriteLine("2. 12:00-14:00 " + (selectedDoctor.AppointmentSlots[1] ? "Available" : "Booked"));
                        Console.WriteLine("3. 15:00-17:00 " + (selectedDoctor.AppointmentSlots[2] ? "Available" : "Booked"));

                        Console.Write("\nEnter your appointment slot choice (1-3): ");
                        if (!int.TryParse(Console.ReadLine(), out int slotChoice) || slotChoice < 1 || slotChoice > 3)
                        {
                            Console.WriteLine("Invalid slot choice. Please enter a number between 1 and 3.");
                            continue;
                        }
                        int slotIndex = slotChoice - 1;

                        if (selectedDoctor.AppointmentSlots[slotIndex])
                        {
                            selectedDoctor.AppointmentSlots[slotIndex] = false;

                            Patient patient = CheckForPatient(patients, departmentChoice, doctorChoice, slotIndex);
                            if (patient != null)
                            {
                                Console.WriteLine($"Sorry, {patient.FirstName} {patient.LastName} is already scheduled for this appointment. Please try another time.");
                            }
                            else
                            {
                                patients.Add(new Patient { FirstName = firstName, LastName = lastName, Email = email, Phone = phone, Department = departmentChoice, Doctor = doctorChoice, Slot = slotIndex });
                                SavePatients(patients);
                                Console.WriteLine($"\nThank you {firstName} {lastName}, you have successfully scheduled an appointment with Dr. {selectedDoctor.FirstName} {selectedDoctor.LastName} at {GetAppointmentTime(slotIndex)}.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nSorry, that appointment slot is already booked. Please try again.");
                        }

                        Console.WriteLine();
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Input format error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error: {ex.Message}");
            }
        }

        static List<Patient> LoadPatients()
        {
            List<Patient> patients = new List<Patient>();
            try
            {
                if (File.Exists("patients.txt"))
                {
                    string[] lines = File.ReadAllLines("patients.txt");
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        patients.Add(new Patient
                        {
                            FirstName = parts[0],
                            LastName = parts[1],
                            Email = parts[2],
                            Phone = parts[3],
                            Department = int.Parse(parts[4]),
                            Doctor = int.Parse(parts[5]),
                            Slot = int.Parse(parts[6])
                        });
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error loading patients: {ex.Message}");
            }
            return patients;
        }

        static void SavePatients(List<Patient> patients)
        {
            try
            {
                File.WriteAllLines("patients.txt", patients.Select(p => $"{p.FirstName},{p.LastName},{p.Email},{p.Phone},{p.Department},{p.Doctor},{p.Slot}"));
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error saving patients: {ex.Message}");
            }
        }

        static Patient CheckForPatient(List<Patient> patients, int departmentChoice, int doctorChoice, int slotIndex)
        {
            foreach (Patient patient in patients)
            {
                if (patient.Department == departmentChoice && patient.Doctor == doctorChoice && patient.Slot == slotIndex)
                {
                    return patient;
                }
            }
            return null;
        }

        static string GetAppointmentTime(int slotIndex)
        {
            switch (slotIndex)
            {
                case 0:
                    return "09:00";
                case 1:
                    return "12:00";
                case 2:
                    return "15:00";
                default:
                    return "";
            }
        }
    }
}
   

    

