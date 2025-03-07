using BAL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class UserMenu : IUserMenu
{
    private readonly IUserMenuRepository _userMenuRepository;

    public UserMenu(IUserMenuRepository userMenuRepository)
    {
        _userMenuRepository = userMenuRepository;
    }
    public List<Categoryviewmodel> GetCategories()
    {
        var categories = _userMenuRepository.GetCategories();
        var categoryViewModels = categories.Select(c => new Categoryviewmodel
        {
            CategoryId = c.Categoryid,
            CategoryName = c.Name,
            Description = c.Description
        }).ToList();

        return categoryViewModels;
    }

     public List<Unit> GetUnits()
     {
        var units = _userMenuRepository.GetUnits();
        return units;
     }

     public async Task<paginationviewmodel> GetItemsByCategory(int id , int pageNo = 1 , int pageSize=3 )
     {
        var items = await _userMenuRepository.GetItemsByCategory(id,pageNo,pageSize);

        return items;
     }

    public async Task<int> GetItemsCountByCategory(int categoryId)
    {
        return await _userMenuRepository.GetCount(categoryId);
    }

     public async Task<bool> AddCategory(Categoryviewmodel model)
     {
         return await _userMenuRepository.AddCategory(model);
     }
     
    public async Task<bool> GetModifierById(int id)
    {
        var existingCategory = await _userMenuRepository.GetUserByIdForDelete(id);

        if(existingCategory == null)
        {
            return false;
        }
        existingCategory.IsDeleted = true;  

         await _userMenuRepository.DeleteCategoryAsync(existingCategory);

         return true;
    }

    public async Task<bool> UpdateCategory(Categoryviewmodel model)
    {
        var category = await _userMenuRepository.GetCategoryId(model.CategoryId);

        if(category == null)
        {
            return false;
        }

        category.Name = model.CategoryName;
        category.Description = model.Description;

        await _userMenuRepository.UpdateCategoryAsync(category);
        return true;
    }
    
     public async Task<bool> AddNewItem(menuviewmodel model)
     {
        if(model.Additem==null)
        {
            return false;
        }
        await _userMenuRepository.AddNewItem(model);
        return true;
     }

     public async Task<EditItemviewmodel> GetEditItem(int id)
     {
        return await _userMenuRepository.GetEditItem(id);
     }

      public async Task<bool> EditItem(menuviewmodel model)
      {
        var existingitem = await _userMenuRepository.GetExistingItem(model.EditItem.Itemid);

         if(existingitem == null)
        {
            return false;
        }
        existingitem.Categoryid = model.EditItem.Categoryid;
        existingitem.Itemid = model.EditItem.Itemid;
        existingitem.Itemname = model.EditItem.Itemname;
        existingitem.Description = model.EditItem.Description;
        existingitem.Quantity = model.EditItem.Quantity;
        existingitem.Rate = model.EditItem.Rate;
        existingitem.IsAvailable = model.EditItem.IsAvailable;
        existingitem.ShortCode = model.EditItem.ShortCode;
        existingitem.Unit = model.EditItem.Unit;
        existingitem.TaxPercentage = model.EditItem.TaxPercentage;
        existingitem.Image = model.EditItem.Image;

         await _userMenuRepository.UpdateItemAsync(existingitem);
         return true;
      }

    

}
