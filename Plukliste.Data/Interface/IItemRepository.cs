using Plukliste.Model.Entity;

namespace Plukliste.Data;

public interface IItemRepository
{
    List<Item> GetAllItems();
    void UpdateAmountForItems(List<Item> items);

    List<Item> SetCurrentAmountForItems(List<Item> items);

    void DecrementStockForItems(List<Item> items);
}
