
namespace Plukliste.Model.Entity;

public class Item
{
    public int Id {get; set;}

    public string ProductID {get; set;}

    public string Title {get; set;}


    public ItemType ItemType {get; set;}

    public int Amount {get; set;}

    public int StockAmount { get; set; }
    
}