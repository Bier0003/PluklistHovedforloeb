using Plukliste.Model.Entity;

namespace Plukliste.BLL;
public class PluklistePrinter
{
    public string GetPluklisteLine(Item item, bool printStock)
    {
        return printStock ? $"{item.Amount,-7}{item.ItemType,-9}{item.ProductID,-20}{item.Title, -25}{item.StockAmount}" : $"{item.Amount,-7}{item.ItemType,-9}{item.ProductID,-20}{item.Title}";
    }


    public string GetPluklisteHeadline(bool printStock)
    {
        return printStock ? $"\n{"Antal",-7}{"Type",-9}{"Produktnr.",-20}{"Navn",-25}{"Lager"}" : $"\n{"Antal",-7}{"Type",-9}{"Produktnr.",-20}{"Navn"}";
    }
}