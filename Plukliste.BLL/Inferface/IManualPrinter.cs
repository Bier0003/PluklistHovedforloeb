namespace Plukliste.BLL.Interface;
public interface IManualPrinter
{
    bool CanPrint(object item);
    void PrintManual(string manualTypeName, object item, bool printOrderContent);
}
