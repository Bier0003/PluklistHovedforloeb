using Plukliste.BLL.Filehandler;
using Plukliste.BLL.Interface;
using Plukliste.BLL.ManualPrinter;
using Plukliste.Model.Entity;

namespace Plukliste;

public class PluklisteConsole
{
    //Arrange
    private char _readKey = ' ';
    private List<string> _files;
    private List<IFileHandler> _fileHandlers;
    private List<IManualPrinter> _manualPrinters;
    private int _index = -1;
    private ConsoleColor standardColor = Console.ForegroundColor;
    

    public PluklisteConsole(IEnumerable<IFileHandler> fileHandlers, IEnumerable<IManualPrinter> manualPrinters)
    {
        _fileHandlers = fileHandlers.ToList();
        _manualPrinters = manualPrinters.ToList();
    }

    public void StartConsoleLoop()
    {
        Directory.CreateDirectory("import");
        Directory.CreateDirectory("print");

        this._files = readFilesFromDirectory("export");

        //ACT
        while (_readKey != 'Q')
        {
            object currentDeserializedFileObject = null;
            IFileHandler fileHandler = null;

            if (_files.Count == 0)
            {
                Console.WriteLine("No files found.");

            }
            else
            {
                if (_index == -1) _index = 0;
                var fileExtension = Path.GetExtension(_files[_index]); //Get the fileextension
                var fileName = Path.GetFileNameWithoutExtension(_files[_index]); //Get the filename
                fileHandler = _fileHandlers.SingleOrDefault(iFileHandler => iFileHandler.CanHandle(fileExtension)) ?? throw new FileLoadException($"File: {_files[_index]} cannot be handled as it is not supported."); //Find the handler that can handle the file type

                Console.WriteLine($"{fileHandler.PrintType()} {_index + 1} af {_files.Count}");
                Console.WriteLine($"\nfile: {_files[_index]}");

                //read file
                using (FileStream file = File.OpenRead(_files[_index]))
                {
                    fileHandler.PrintContent(file, fileName);
                    currentDeserializedFileObject = fileHandler.GetDeserializedObject();
                }
            }

            //Print options
            Console.WriteLine("\n\nOptions:");
            PrintOption("Q", "uit", standardColor);

            if (_index >= 0)
            {
                PrintOption("A", "fslut fil", standardColor);
            }
            if (_index > 0)
            {
                PrintOption("F", "orrige fil", standardColor);
            }
            if (_index < _files.Count - 1)
            {
                PrintOption("N", "æste fil", standardColor);
            }

            PrintOption("G", "enindlæs filer", standardColor);

            if (currentDeserializedFileObject is Pluklist) //TODO: print options based on type in a seperate class
            {

                PrintOption("O", "pgraderingsvejledning udskrift", standardColor);

                PrintOption("U", "psigelsesvejledning udskrift", standardColor);

                PrintOption("V", "elkomst udskrift", standardColor);
            }

            _readKey = Console.ReadKey().KeyChar;
            if (_readKey >= 'a') _readKey -= (char)('a' - 'A');
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;

            var manualPrinterForObject = _manualPrinters.SingleOrDefault(p => p.CanPrint(currentDeserializedFileObject));

            switch (_readKey) //TODO: optons to be based on type in a seperate class instead
            {
                case 'G':
                    _files = Directory.EnumerateFiles("export").ToList();
                    _index = -1;
                    Console.WriteLine("Filer genindlæst");
                    break;
                case 'F':
                    if (_index > 0) _index--;
                    break;
                case 'N':
                    if (_index < _files.Count - 1) _index++;
                    break;
                case 'A':
                    //Move files to import directory
                    fileHandler.FinalizeFile(currentDeserializedFileObject);
                    var filewithoutPath = _files[_index].Substring(_files[_index].LastIndexOf('\\'));
                    File.Move(_files[_index], string.Format(@"import\\{0}", filewithoutPath));
                    Console.WriteLine($"Fil {_files[_index]} afsluttet.");
                    _files.Remove(_files[_index]);
                    if (_index == _files.Count) _index--;
                    break;
                case 'O': //Opgradeingsvejledning
                    if (currentDeserializedFileObject is null) break;
                    manualPrinterForObject.PrintManual("OPGRADE", currentDeserializedFileObject, true);
                    Console.WriteLine("Opgraderingsvejledning sendt til print!");
                    break;
                case 'U': //Opsigelsesvejledning
                    if (currentDeserializedFileObject is null) break;
                    manualPrinterForObject.PrintManual("OPSIGELSE", currentDeserializedFileObject, false);
                    Console.WriteLine("Opsigelsesvejledning sendt til print!");
                    break;
                case 'V': //Velkomst vejledning
                    if (currentDeserializedFileObject is null) break;
                    manualPrinterForObject.PrintManual("WELCOME", currentDeserializedFileObject, true);
                    Console.WriteLine("Velkomst sendt til print!");
                    break;
            }
            Console.ForegroundColor = standardColor; //reset color
        }
    }


    void PrintOption(string optionLetter, string followupText, ConsoleColor foregroundColor)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(optionLetter);
        Console.ForegroundColor = foregroundColor;
        Console.WriteLine(followupText);
    }

    List<string> readFilesFromDirectory(string directoryName)
    {
        if (!Directory.Exists(directoryName))
        {
            Console.WriteLine($"Directory \"{directoryName}\" not found");
            Console.ReadLine();
            return null;
        }
        return Directory.EnumerateFiles(directoryName).ToList(); ;
    }
}

