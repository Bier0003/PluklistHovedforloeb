using Plukliste.Model.Entity;

namespace Plukliste.Model;
public class PluklistePrinter
{
    public string GetPluklisteLine(Item item)
    {
        return $"{item.Amount,-7}{item.Type,-9}{item.ProductID,-20}{item.Title}";
    }


    public string GetPluklisteHeadline()
    {
        return $"\n{"Antal",-7}{"Type",-9}{"Produktnr.",-20}{"Navn"}";
    }
}