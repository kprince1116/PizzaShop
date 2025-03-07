using BAL.Interfaces;
using DAL.Interfaces;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.Presentation.Controllers;

public class MenuController : Controller
{

    private readonly IUserMenu _userMenu;

    public MenuController(IUserMenu userMenu)
    {
        _userMenu = userMenu;
    }

    [HttpGet]
    [Route("Menu/Items")]
    public async Task<IActionResult> Items(int id, int pageNo = 1 , int pageSize=3)
    {
        var categories = _userMenu.GetCategories();
        var units = _userMenu.GetUnits();
        int selectedCategoryId = id != 0 ? id : categories.First().CategoryId;

        var items = await _userMenu.GetItemsByCategory(selectedCategoryId,  pageNo ,  pageSize);

        var viewModel = new menuviewmodel
        {
            Categories = categories,
            pagination = items,
            units = units
        };

        // ViewBag.SearchTerm = searchTerm;
        // ViewBag.SelectedCategoryId = selectedCategoryId;

        return View(viewModel);

    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(Categoryviewmodel model)
    {
        var isAdded = await _userMenu.AddCategory(model);
        if (isAdded)
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return Content("error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditCategory(Categoryviewmodel model)
    {
        await _userMenu.UpdateCategory(model);
        return RedirectToAction("Index", "Home");
    }


    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var existingCategory = await _userMenu.GetModifierById(id);

        return RedirectToAction("Index", "Home");

    }

    [Route("Menu/ItemsByCategory")]
    public async Task<IActionResult> ItemsByCategory(int id , int pageNo = 1 , int pageSize=3 )
    {
        var items = await _userMenu.GetItemsByCategory(id,pageNo,pageSize);
        return PartialView("_ItemsPartial", items);
    }

    // public async Task<IActionResult> GetItemList( int categoryId=1 , int pageNo=1 , int pageSize , string search = ""  )
    // {
    //     return PartialView("_ItemsPartial", await _userMenu.GetItemList(categoryId,pageNo,pageSize,search));
    // }

    [HttpPost]
    public async Task<IActionResult> Items(menuviewmodel model)
    {
        var isAdded = await _userMenu.AddNewItem(model);

        return RedirectToAction("Index","Home");
    }

     public async Task<IActionResult> EditItem(int id)
    {
        var item =await _userMenu.GetEditItem(id);

        return PartialView("_EditItemPartial",item);                                 
    }

    [HttpPost]
    public async Task<IActionResult> EditItems(menuviewmodel model)
    {
        var item = _userMenu.EditItem(model);
        return View(model);                                  
    }

    public IActionResult Modifiers()
    {
        return View();
    }
}