using Plukliste.Model.Entity;
using Plukliste.Model.Interface;

namespace Plukliste.Model.Filehandler;
public class XMLFileHandler : IFileHandler
{
    private Pluklist _plukliste = null;
    private PluklistePrinter _pluklistePrinter = new PluklistePrinter();
    public bool CanHandle(string filetype)
    {
        return filetype.ToUpper().Equals(".XML");
    }

    public object GetDeserializedObject()
    {
        return _plukliste;
    }

    public Type GetHandleType()
    {
        return typeof(Pluklist);
    }

    public void PrintContent(FileStream fileStream, string fileName)
    {
        
        System.Xml.Serialization.XmlSerializer xmlSerializer =
                        new System.Xml.Serialization.XmlSerializer(typeof(Pluklist));
                    _plukliste = (Pluklist?)xmlSerializer.Deserialize(fileStream);

                    //print plukliste
                    if (_plukliste != null && _plukliste.Lines != null)
                    {
                        Console.WriteLine($"\n{"Name:",-13}{_plukliste.Name}");
                        Console.WriteLine($"{"Forsendelse:",-13}{_plukliste.Forsendelse}");
                        Console.WriteLine($"{"Addresse:",-13}{_plukliste.Adresse}");

                        Console.WriteLine(_pluklistePrinter.GetPluklisteHeadline());
                        foreach (var item in _plukliste.Lines)
                        {
                            Console.WriteLine(_pluklistePrinter.GetPluklisteLine(item));
                        }
                    }
    }

    public string PrintType()
    {
        return "Plukliste ";
    }
}
