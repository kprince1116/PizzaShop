using DAL.Models;
using DAL.ViewModels;
using Pizzashop.DAL.ViewModels;

namespace BAL.Interfaces;

public interface IUserMenu
{
    public List<Categoryviewmodel> GetCategories();

    public List<Unit> GetUnits();
    public Task<paginationviewmodel> GetItemsByCategory(int id , int pageNo  , int pageSize );

    // public Task<int> GetItemsCountByCategory(int categoryId);

    public Task<bool> AddCategory(Categoryviewmodel model);

    public Task<bool> GetModifierById(int id);

    public Task<bool> UpdateCategory(Categoryviewmodel model);

    public Task<bool> AddNewItem( menuviewmodel model);
    
    public Task<EditItemviewmodel> GetEditItem(int id);
    public Task<bool> EditItem(menuviewmodel Model);

    // Task UpdateItemAsync(MenuItemsviewmodel existingitem);
    // public Task<ItemListviewmodel>  GetItemList(int categoryId,int pageNo,int pageSize,string search);
  
}
