using Microsoft.EntityFrameworkCore;
using Plukliste.Model.Entity;

namespace Plukliste.Data;

public class SQLPluklisteRepository : IPluklisteRepository
{
    private IDbContextFactory<PluklistDbContext> _dbContextFactory;
    public SQLPluklisteRepository(IDbContextFactory<PluklistDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public List<Pluklist> GetAllPluklists()
    {
        using(var context = _dbContextFactory.CreateDbContext())
        {
            return context.Pluklist.ToList();
        }
    }
}