using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Plukliste.Model.Entity;

namespace Plukliste.Data;

public class SQLItemRepository : IItemRepository
{
    private IDbContextFactory<PluklistDbContext> _dbContextFactory;

    public SQLItemRepository(IDbContextFactory<PluklistDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public void DecrementStockForItems(List<Item> items)
    {
        using(var context = _dbContextFactory.CreateDbContext())
        {
            foreach(var item in items)
            {
                var dbItem = context.Item.SingleOrDefault(x => x.ProductID == item.ProductID && item.Title == x.Title);
                if(dbItem != null)
                {
                    dbItem.Amount -= item.Amount;
                    context.Update(dbItem);
                }
            }
            context.SaveChanges();
        }
    }

    public List<Item> GetAllItems()
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {

            return context.Item.ToList();
        }
    }

    public List<Item> SetCurrentAmountForItems(List<Item> items)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            foreach (var item in items)
            {
                var dbItem = context.Item.SingleOrDefault(x => x.ProductID == item.ProductID && item.Title == x.Title);
                item.StockAmount = dbItem is not null ? dbItem.Amount : 0;
            }
        }
        return items;
    }

    public void UpdateAmountForItems(List<Item> items)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            foreach (var item in items)
            {
                context.Item
                    .Where(x => x.Id == item.Id)
                    .ExecuteUpdate(x => x.SetProperty(y => y.Amount, item.Amount));
            }
            context.SaveChanges();
        }
    }
}