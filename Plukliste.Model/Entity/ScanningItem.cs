using CsvHelper.Configuration.Attributes;

namespace Plukliste.Model.Entity;

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
