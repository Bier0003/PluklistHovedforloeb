using Plukliste.Model.Entity;

namespace Plukliste.Data;

public interface IPluklisteRepository
{
    List<Pluklist> GetAllPluklists();
}