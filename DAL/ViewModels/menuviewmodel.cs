using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace  Pizzashop.DAL.ViewModels;

public class menuviewmodel
{
        public IEnumerable<Categoryviewmodel> Categories { get; set; }
        
        // public IEnumerable<MenuItemsviewmodel> Items { get; set; }
        public AddItemviewmodel Additem {get; set;}
        public EditItemviewmodel EditItem {get; set;}
        public IEnumerable<Unit> units {get; set;} 
        public paginationviewmodel pagination {get; set;}
}
