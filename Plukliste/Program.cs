using System.Text;

namespace Plukliste;

class PluklisteProgram { 
    
    static void Main()
    {
        //Arrange
        char readKey = ' ';
        List<string> files;
        //List<string> templateFiles;
        var index = -1;
        var standardColor = Console.ForegroundColor;
        Directory.CreateDirectory("import");
        Directory.CreateDirectory("print");
                
        files = readFilesFromDirectory("export");
        //templateFiles = readFilesFromDirectory("templates");

        //ACT
        while (readKey != 'Q')
        {
            Pluklist plukliste = null;
            if (files.Count == 0)
            {
                Console.WriteLine("No files found.");

            }
            else
            {
                if (index == -1) index = 0;

                Console.WriteLine($"Plukliste {index + 1} af {files.Count}");
                Console.WriteLine($"\nfile: {files[index]}");

                //read file
                using(FileStream file = File.OpenRead(files[index])) 
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer =
                        new System.Xml.Serialization.XmlSerializer(typeof(Pluklist));
                    plukliste = (Pluklist?)xmlSerializer.Deserialize(file);

                    //print plukliste
                    if (plukliste != null && plukliste.Lines != null)
                    {
                        Console.WriteLine($"\n{"Name:",-13}{plukliste.Name}");
                        Console.WriteLine($"{"Forsendelse:",-13}{plukliste.Forsendelse}");
                        Console.WriteLine($"{"Addresse:",-13}{plukliste.Adresse}");

                        Console.WriteLine(GetPluklisteHeadline());
                        foreach (var item in plukliste.Lines)
                        {
                            Console.WriteLine(GetPluklisteLine(item));
                        }
                    }
                }
            }

            //Print options
            Console.WriteLine("\n\nOptions:");
            PrintOption("Q", "uit", standardColor);

            if (index >= 0)
            {
                PrintOption("A", "fslut plukseddel", standardColor);
            }
            if (index > 0)
            {
                PrintOption("F", "orrige plukseddel", standardColor);
            }
            if (index < files.Count - 1)
            {
                PrintOption("N", "æste plukseddel", standardColor);
            }

            PrintOption("G", "enindlæs pluksedler", standardColor);

            PrintOption("O", "pgraderingsvejledning udskrift", standardColor);

            PrintOption("U", "psigelsesvejledning udskrift", standardColor);

            PrintOption("V", "elkomst udskrift", standardColor);

            readKey = Console.ReadKey().KeyChar;
            if (readKey >= 'a') readKey -= (char)('a' - 'A'); //HACK: To upper
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red; //status in red
            switch (readKey)
            {
                case 'G':
                    files = Directory.EnumerateFiles("export").ToList();
                    index = -1;
                    Console.WriteLine("Pluklister genindlæst");
                    break;
                case 'F':
                    if (index > 0) index--;
                    break;
                case 'N':
                    if (index < files.Count - 1) index++;
                    break;
                case 'A':
                    //Move files to import directory
                    var filewithoutPath = files[index].Substring(files[index].LastIndexOf('\\'));
                    File.Move(files[index], string.Format(@"import\\{0}", filewithoutPath));
                    Console.WriteLine($"Plukseddel {files[index]} afsluttet.");
                    files.Remove(files[index]);
                    if (index == files.Count) index--;
                    break;
                case 'O': //Opgradeingsvejledning
                    PrintManual("OPGRADE", plukliste, true);
                    Console.WriteLine("Opgraderingsvejledning sendt til print!");
                    break;
                case 'U': //Opsigelsesvejledning
                    PrintManual("OPSIGELSE", plukliste, false);
                    Console.WriteLine("Opsigelsesvejledning sendt til print!");
                    break;
                case 'V': //Velkomst vejledning
                    PrintManual("WELCOME", plukliste, true);
                    Console.WriteLine("Velkomst sendt til print!");
                    break;
            }
            Console.ForegroundColor = standardColor; //reset color

        }
    }

    private static string GetPluklisteLine(Item item)
    {
        return $"{item.Amount,-7}{item.Type,-9}{item.ProductID,-20}{item.Title}";
    }


    private static string GetPluklisteHeadline()
    {
        return $"\n{"Antal",-7}{"Type",-9}{"Produktnr.",-20}{"Navn"}";
    }


    public static void PrintManual(string type, Pluklist plukliste, bool printPlukliste)
    {
        var content = File.ReadAllText("templates/PRINT-" + type + ".html");
        content = content.Replace("[Adresse]", plukliste.Adresse);
        content = content.Replace("[Name]", plukliste.Name);
        if(printPlukliste)
        {
            var sb = new StringBuilder();
            sb.Append("<pre>");
            sb.AppendLine(GetPluklisteHeadline());
            foreach(var item in plukliste.Lines)
                sb.AppendLine(GetPluklisteLine(item));
            sb.Append("</pre>");
            content = content.Replace("[Plukliste]", sb.ToString());
        }
        File.WriteAllText("print/" + Guid.NewGuid().ToString() + ".html", content);
    }

    public static void PrintOption(string optionLetter, string followupText, ConsoleColor foregroundColor)
    {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(optionLetter);
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(followupText);
    }

    public static List<string> readFilesFromDirectory(string directoryName)
    {
        if (!Directory.Exists(directoryName))
        {
            Console.WriteLine($"Directory \"{directoryName}\" not found");
            Console.ReadLine();
            return null;
        }
        return Directory.EnumerateFiles(directoryName).ToList();;
    }
}
