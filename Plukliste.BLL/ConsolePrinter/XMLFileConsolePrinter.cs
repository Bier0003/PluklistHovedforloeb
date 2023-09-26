using Plukliste.Model.Entity;
using Plukliste.BLL.Interface;

namespace Plukliste.Model.ConsolePrinter;
public class XMLFileConsolePrinter : IPrinter
{
    public bool CanPrint(object type)
    {
        return type is Pluklist;
    }
}
