using Plukliste.Model.Entity;
using Plukliste.BLL.Interface;
using Plukliste.Data;

namespace Plukliste.BLL.Filehandler;
public class XMLFileHandler : IFileHandler
{
    private Pluklist _plukliste = null;
    private PluklistePrinter _pluklistePrinter = new PluklistePrinter();

    private IItemRepository _itemRepository;


    public XMLFileHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public bool CanHandle(string filetype)
    {
        return filetype.ToUpper().Equals(".XML");
    }

    public void FinalizeFile(object file)
    {
        if(file is not Pluklist || (file as Pluklist).Lines == null)
            return;
        _itemRepository.DecrementStockForItems((file as Pluklist).Lines);
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
                        _plukliste.Lines = _itemRepository.SetCurrentAmountForItems(_plukliste.Lines);

                        Console.WriteLine($"\n{"Name:",-13}{_plukliste.Name}");
                        Console.WriteLine($"{"Forsendelse:",-13}{_plukliste.Forsendelse}");
                        Console.WriteLine($"{"Addresse:",-13}{_plukliste.Adresse}");

                        Console.WriteLine(_pluklistePrinter.GetPluklisteHeadline(true));
                        foreach (var item in _plukliste.Lines)
                        {
                            Console.WriteLine(_pluklistePrinter.GetPluklisteLine(item, true));
                        }
                    }
    }

    public string PrintType()
    {
        return "Plukliste ";
    }
}
