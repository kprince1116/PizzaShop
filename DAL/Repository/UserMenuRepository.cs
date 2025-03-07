using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Pizzashop.DAL.ViewModels;

namespace DAL.Repository;

public class UserMenuRepository : IUserMenuRepository
{

    private readonly PizzaShopContext _db;

    public UserMenuRepository(PizzaShopContext db)
    {
        _db = db;
    }
    public List<MenuCategory> GetCategories()
    {
        return _db.MenuCategories.Where(u => u.IsDeleted == false).ToList();
    }

    public List<Unit> GetUnits()
    {
        return _db.Units.ToList();
    }

    public async Task<paginationviewmodel> GetItemsByCategory(int id ,  int pageNo = 1 , int pageSize=3 )
    {
        var item = _db.MenuItems.Where(u=> u.Categoryid == id ).Select(u=> new MenuItemsviewmodel{
            Itemid = u.Itemid,
            Categoryid = u.Categoryid,
            Itemname = u.Itemname,
            Itemtype = u.Itemtype,
            Description = u.Description,
            Rate = u.Rate,
            Quantity = u.Quantity,
            IsAvailable = u.IsAvailable,
            Image = u.Image,
        });

        var totalRecords =  item.Count();

        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize); 

        List<MenuItemsviewmodel> items = await item.Skip((pageNo-1)*pageSize).Take(pageSize).ToListAsync();

        return new paginationviewmodel
    {
        Items = items,
        TotalRecords = totalRecords,
        TotalPages = totalPages,
        CurrentPage = pageNo,
        PageSize = pageSize
    };
    }

     public Task<int> GetCount(int categoryId)
     {
             var query = _db.MenuItems;
            
            return query.CountAsync();
     }

    public async Task<bool> AddCategory(Categoryviewmodel model)
    {
        var category = new MenuCategory
        {
            Name = model.CategoryName,
            Description = model.Description,
        };

        _db.MenuCategories.Add(category);
        bool success =  await _db.SaveChangesAsync() > 0;
        return success;
    }

    public Task<MenuCategory> GetUserByIdForDelete(int id)
    {
        return _db.MenuCategories.FirstOrDefaultAsync(m => m.Categoryid == id);
    }

    public async Task DeleteCategoryAsync(MenuCategory existingCategory)
    {
        _db.MenuCategories.Update(existingCategory);
        await _db.SaveChangesAsync();
    }

    public  Task<MenuCategory> GetCategoryId(int id)
    {
        return _db.MenuCategories.FirstOrDefaultAsync(c => c.Categoryid == id);
    }

    public async Task UpdateCategoryAsync(MenuCategory category)
    {
        _db.MenuCategories.Update(category);
        await _db.SaveChangesAsync();
     }

    public async Task AddNewItem(menuviewmodel model)
    {

        var item = new MenuItem{
            Categoryid = model.Additem.Categoryid,
            Itemname = model.Additem.Itemname,
            Itemtype = model.Additem.Itemtype,
            Quantity = model.Additem.Quantity,
            Rate = model.Additem.Rate,
            IsAvailable = model.Additem.IsAvailable,
            Image = model.Additem.Image,
            Description=model.Additem.Description,
            TaxPercentage = model.Additem.TaxPercentage,
        };


        _db.MenuItems.Update(item);
        await _db.SaveChangesAsync();
    }

    public async Task< EditItemviewmodel> GetEditItem(int id)
    {
        return await _db.MenuItems.Where(u=>u.Itemid == id).Select( u=> new EditItemviewmodel
        {
            Categoryid = u.Categoryid,
            Itemname = u.Itemname,
            Itemtype = u.Itemtype,
            Quantity = u.Quantity,
            Rate = u.Rate,
            IsAvailable = u.IsAvailable,
            Image = u.Image,
            Description=u.Description,
            TaxPercentage = u.TaxPercentage,
            Categories =  _db.MenuCategories.ToList(),
            units =  _db.Units.ToList()
        }
        ).FirstOrDefaultAsync();
    }
    public async Task<MenuItem> GetExistingItem(int id)
    {
        return await _db.MenuItems.FirstOrDefaultAsync(u=>u.Itemid == id);
    }

    public async Task UpdateItemAsync(MenuItem existingitem)
    {
        _db.MenuItems.Update(existingitem);
        await _db.SaveChangesAsync();
    }

}
