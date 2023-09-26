namespace Plukliste.BLL.Interface;
public interface IFileHandler
{
    bool CanHandle(string filetype);

    void PrintContent(FileStream fileStream, string fileName);

    string PrintType();

    object GetDeserializedObject();

    Type GetHandleType();

    void FinalizeFile(object file);
}
