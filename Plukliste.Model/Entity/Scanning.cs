using CsvHelper.Configuration.Attributes;
using Plukliste.Model.Entity;

namespace Plukliste.Model;
public class Scanning
{
    public Scanning()
    {
        ScanningItems = new List<ScanningItem>();
        Forsendelse = "Pickup";
    }
    public string ScannerName { get; set; }

    public string Forsendelse { get; set; }

    public List<ScanningItem> ScanningItems { get; set; }
}

public class ScanningItem
{
    [Index(0)]
    public string ProductID { get; set; }

    [Index(1)]
    public ItemType Type { get; set; }

    [Index(2)]
    public string Description { get; set; }

    [Index(3)]
    public int Amount { get; set; }
}
