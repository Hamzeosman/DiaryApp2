public class DiaryEntry
{
    public DateTime Date { get; set; }
    public string Text { get; set; }

    public override string ToString()
    {
        return $"{Date.ToShortDateString()}: {Text}";
    }
}


class Program
{
    static List<DiaryEntry> diaryEntries = new List<DiaryEntry>(); // Samlar alla anteckningar
    static Dictionary<DateTime, DiaryEntry> diaryDictionary = new Dictionary<DateTime, DiaryEntry>(); // För snabbare sökning på datum

    static string filePath = "diary.txt"; // Filen där anteckningar sparas

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Hamzes Diary ===");
            Console.WriteLine("1. Write New Notes");
            Console.WriteLine("2. List All Notes");
            Console.WriteLine("3. Search For Notes By Date");
            Console.WriteLine("4. Save To File");
            Console.WriteLine("5. Read From File");
            Console.WriteLine("6. Exit");
            Console.Write("Choose Another Alternative!: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    AddEntry();
                    break;
                case "2":
                    ListEntries();
                    break;
                case "3":
                    SearchEntry();
                    break;
                case "4":
                    SaveToFile();
                    break;
                case "5":
                    LoadFromFile();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid Choice, Press Optional Key To Continue!.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void AddEntry()
    {
        Console.Clear();
        Console.WriteLine("=== NEW NOTE ===");

        DateTime date;
        Console.Write("Enter date (åååå-mm-dd): ");
        while (!DateTime.TryParse(Console.ReadLine(), out date))
        {
            Console.Write("Invalid date, Try Again (åååå-mm-dd): ");
        }

        Console.Write("Write Your New Note: ");
        string text = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("Note Cannot Be Empty!");
            Console.ReadKey();
            return;
        }

        DiaryEntry entry = new DiaryEntry { Date = date, Text = text };
        diaryEntries.Add(entry);

        // Om datumet redan finns, ersätt i dictionary
        diaryDictionary[date] = entry;

        Console.WriteLine("New Note Saved!");
        Console.ReadKey();
    }

    static void ListEntries()
    {
        Console.Clear();
        Console.WriteLine("=== ALL NOTES ===");

        if (diaryEntries.Count == 0)
        {
            Console.WriteLine("No Notes Found.");
        }
        else
        {
            foreach (var entry in diaryEntries)
            {
                Console.WriteLine(entry);
            }
        }

        Console.ReadKey();
    }
    static void SearchEntry()
    {
        Console.Clear();
        Console.WriteLine("=== FIND NOTES ===");

        Console.Write("Enter Date (åååå-mm-dd): ");
        DateTime date;
        if (!DateTime.TryParse(Console.ReadLine(), out date))
        {
            Console.WriteLine("Invalid date!");
        }
        else if (diaryDictionary.ContainsKey(date))
        {
            Console.WriteLine(diaryDictionary[date]);
        }
        else
        {
            Console.WriteLine("No Notes Found For This Date.");
        }

        Console.ReadKey();
    }

    static void SaveToFile()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var entry in diaryEntries)
                {
                    writer.WriteLine($"{entry.Date}|{entry.Text}");
                }
            }
            Console.WriteLine("Notes Saved To File!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Saving Error: {ex.Message}");
        }
        Console.ReadKey();
    }

    static void LoadFromFile()
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File Does Not Exist!");
                Console.ReadKey();
                return;
            }

            diaryEntries.Clear();
            diaryDictionary.Clear();

            foreach (var line in File.ReadAllLines(filePath))
            {
                string[] parts = line.Split('|');
                if (parts.Length == 2 && DateTime.TryParse(parts[0], out DateTime date))
                {
                    DiaryEntry entry = new DiaryEntry { Date = date, Text = parts[1] };
                    diaryEntries.Add(entry);
                    diaryDictionary[date] = entry;
                }
                else
                {
                    Console.WriteLine($"Invalid Line In File: {line}");
                }
            }
            Console.WriteLine("Notes Loaded From File!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error While Loading: {ex.Message}");
        }
        Console.ReadKey();
    }
}