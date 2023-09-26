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
