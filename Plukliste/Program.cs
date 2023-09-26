using Plukliste.Model.Entity;
using Plukliste.Model.Filehandler;
using Plukliste.Model.Interface;
using Plukliste.Model.ManualPrinter;

namespace Plukliste;

class PluklisteProgram { 
    
    static void Main()
    {
        //Arrange
        char readKey = ' ';
        List<string> files;
        var fileHandlers = new List<IFileHandler>() { new XMLFileHandler(), new CSVFileHandler() };
        var manualPrinters = new List<IManualPrinter>() { new PluklisteManualPrinter() };
        var index = -1;
        var standardColor = Console.ForegroundColor;
        Directory.CreateDirectory("import");
        Directory.CreateDirectory("print");
                
        files = readFilesFromDirectory("export");

        //ACT
        while (readKey != 'Q')
        {
            object currentDeserializedFileObject = null;

            if (files.Count == 0)
            {
                Console.WriteLine("No files found.");

            }
            else
            {
                if (index == -1) index = 0;
                var fileExtension = Path.GetExtension(files[index]); //Get the fileextension
                var fileName = Path.GetFileNameWithoutExtension(files[index]); //Get the filename
                var fileHandler = fileHandlers.SingleOrDefault(iFileHandler => iFileHandler.CanHandle(fileExtension)) ?? throw new FileLoadException($"File: {files[index]} cannot be handled as it is not supported."); //Find the handler that can handle the file type

                Console.WriteLine($"{fileHandler.PrintType()} {index + 1} af {files.Count}");
                Console.WriteLine($"\nfile: {files[index]}");

                //read file
                using(FileStream file = File.OpenRead(files[index])) 
                {
                    fileHandler.PrintContent(file, fileName);
                    currentDeserializedFileObject = fileHandler.GetDeserializedObject();
                }
            }

            //Print options
            Console.WriteLine("\n\nOptions:");
            PrintOption("Q", "uit", standardColor);

            if (index >= 0)
            {
                PrintOption("A", "fslut fil", standardColor);
            }
            if (index > 0)
            {
                PrintOption("F", "orrige fil", standardColor);
            }
            if (index < files.Count - 1)
            {
                PrintOption("N", "æste fil", standardColor);
            }

            PrintOption("G", "enindlæs filer", standardColor);

            if(currentDeserializedFileObject is Pluklist) //TODO: print options based on type in a seperate class
            {

                PrintOption("O", "pgraderingsvejledning udskrift", standardColor);

                PrintOption("U", "psigelsesvejledning udskrift", standardColor);

                PrintOption("V", "elkomst udskrift", standardColor);
            }

            readKey = Console.ReadKey().KeyChar;
            if (readKey >= 'a') readKey -= (char)('a' - 'A');
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;

            var manualPrinterForObject = manualPrinters.SingleOrDefault(p => p.CanPrint(currentDeserializedFileObject));

            switch (readKey) //TODO: optons to be based on type in a seperate class instead
            {
                case 'G':
                    files = Directory.EnumerateFiles("export").ToList();
                    index = -1;
                    Console.WriteLine("Filer genindlæst");
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
                    Console.WriteLine($"Fil {files[index]} afsluttet.");
                    files.Remove(files[index]);
                    if (index == files.Count) index--;
                    break;
                case 'O': //Opgradeingsvejledning
                    if(currentDeserializedFileObject is null) break;
                    manualPrinterForObject.PrintManual("OPGRADE", currentDeserializedFileObject, true);
                    Console.WriteLine("Opgraderingsvejledning sendt til print!");
                    break;
                case 'U': //Opsigelsesvejledning
                    if(currentDeserializedFileObject is null) break;
                    manualPrinterForObject.PrintManual("OPSIGELSE", currentDeserializedFileObject, false);
                    Console.WriteLine("Opsigelsesvejledning sendt til print!");
                    break;
                case 'V': //Velkomst vejledning
                    if(currentDeserializedFileObject is null) break;
                    manualPrinterForObject.PrintManual("WELCOME", currentDeserializedFileObject, true);
                    Console.WriteLine("Velkomst sendt til print!");
                    break;
            }
            Console.ForegroundColor = standardColor; //reset color

        }
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