using Microsoft.AspNetCore.Mvc.RazorPages;
using Plukliste.Data;
using Plukliste.Model.Entity;

namespace Plukliste.Web;


public class IndexViewModel : PageModel
{
    private readonly ILogger<IndexViewModel> _logger;
    private readonly IItemRepository _itemRepository;

    public IndexViewModel(ILogger<IndexViewModel> logger, IItemRepository itemRepository)
    {
        _logger = logger;
        _itemRepository = itemRepository;
    }

    public void OnGet()
    {

    }

    public List<Item> GetAllItems() {
        return _itemRepository.GetAllItems();
    }

    public void OnPost(List<Item> items)
    {
        _itemRepository.UpdateAmountForItems(items);
    }
}
