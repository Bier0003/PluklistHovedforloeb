using System.Text;
using Plukliste.Model.Entity;
using Plukliste.BLL.Interface;

namespace Plukliste.BLL.ManualPrinter;
public class PluklisteManualPrinter : IManualPrinter
{
    public bool CanPrint(object item)
    {
        return item is Pluklist;
    }

    public void PrintManual(string manualTypeName, object item, bool printOrderContent)
    {
        if(item is not Pluklist)
            throw new Exception("Unsupported type");
        var plukliste = (Pluklist)item;

        var pluklistePrinter = new PluklistePrinter();
        var content = File.ReadAllText("templates/PRINT-" + manualTypeName + ".html");
        content = content.Replace("[Adresse]", plukliste.Adresse);
        content = content.Replace("[Name]", plukliste.Name);
        if(printOrderContent)
        {
            var sb = new StringBuilder();
            sb.Append("<pre>");
            sb.AppendLine(pluklistePrinter.GetPluklisteHeadline(false));
            foreach(var pluklisteItem in plukliste.Lines)
                sb.AppendLine(pluklistePrinter.GetPluklisteLine(pluklisteItem, false));
            sb.Append("</pre>");
            content = content.Replace("[Plukliste]", sb.ToString());
        }
        File.WriteAllText("print/" + Guid.NewGuid().ToString() + ".html", content);
    }

}
