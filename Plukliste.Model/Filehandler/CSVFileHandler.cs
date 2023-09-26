using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Plukliste.Model.Interface;

namespace Plukliste.Model.Filehandler;
public class CSVFileHandler : IFileHandler
{
    private Scanning _scanning = null;
    public bool CanHandle(string filetype)
    {
        return filetype.ToUpper().Equals(".CSV");
    }

    public object GetDeserializedObject()
    {
        return _scanning;
    }

    public Type GetHandleType()
    {
        return typeof(Scanning);
    }

    public void PrintContent(FileStream fileStream, string fileName)
    {
        var scanning = new Scanning();
        SetDriverNameFromFilename(fileName, scanning);
        using(var textReader = new StreamReader(fileStream))
        using (var csv = new CsvReader(textReader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";"}))
        {
            var records = csv.GetRecords<ScanningItem>().ToList();
            foreach (var record in records) {
                scanning.ScanningItems.Add(record);
            }
        }
        _scanning = scanning;

        if(_scanning.ScanningItems.Any())
        {
            Console.WriteLine($"\n{"Name:",-13}{_scanning.ScannerName}");
            Console.WriteLine($"\n{"Forsendelse:",-13}{_scanning.Forsendelse}");

            Console.WriteLine($"\n{"Antal",-7}{"Type",-9}{"Produktnr.",-20}");

            foreach(var item in _scanning.ScanningItems) {
                Console.WriteLine($"{item.Amount,-7}{item.Type,-9}{item.ProductID,-20}");
            }
        } 
    }

    public string PrintType()
    {
        return "Scanning ";
    }

    private void SetDriverNameFromFilename(string fileName, Scanning scanning)
    {
        var stringSplit = fileName.Split('_');
        var driverName = "";
        for(int i = 1; i < stringSplit.Length; i++) //Start at 1 to skip the number in the beginning of the file.
        {
            driverName += stringSplit[i] + (i + 1 == stringSplit.Length ? "" : " "); //put space between names. don't put space if there are no more names in th list.
        }
        scanning.ScannerName = driverName;
    }
}
