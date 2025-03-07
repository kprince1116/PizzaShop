using DAL.Models;
using DAL.ViewModels;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface IUserMenuRepository
{

    List<MenuCategory> GetCategories();

    List<Unit> GetUnits();

    public Task<paginationviewmodel> GetItemsByCategory(int id,  int pageNo = 1 , int pageSize=3);

    public Task<int> GetCount(int categoryId);

    public Task<bool> AddCategory( Categoryviewmodel model);

    public Task<MenuCategory> GetUserByIdForDelete(int id);

    Task DeleteCategoryAsync(MenuCategory existingCategory);

    public Task<MenuCategory> GetCategoryId(int id);

    Task UpdateCategoryAsync(MenuCategory category);

    Task AddNewItem(menuviewmodel model);

    public Task<EditItemviewmodel> GetEditItem(int id);
    public Task<MenuItem> GetExistingItem(int id);

    Task UpdateItemAsync( MenuItem existingitem);
    // public  Task<(List<MenuItemsviewmodel> items , int TotalRecords)> GetItemList(int categoryId,int pageNo,int pageSize,String search);
}
