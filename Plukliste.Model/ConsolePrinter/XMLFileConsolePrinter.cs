using Plukliste.Model.Entity;
using Plukliste.Model.Interface;

namespace Plukliste.Model.ConsolePrinter;
public class XMLFileConsolePrinter : IPrinter
{
    public bool CanPrint(object type)
    {
        return type is Pluklist;
    }
}
