using Nancy;
using Inventory;
using System.Collections.Generic;

namespace Inventory
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
      List<MenuItem> newList = MenuItem.GetAll();
      return View ["index.cshtml", newList];
      };

      Post["/"] = _ => {
      MenuItem newMenuItem = new MenuItem(Request.Form["menu-item-name"]);
      newMenuItem.Save();
      List<MenuItem> newList = MenuItem.GetAll();
      return View["index.cshtml", newList];
      };

      Get["/menu-item/{id}"] = parameters => {
      var selectedMenuItem = MenuItem.Find(parameters.id);
      return View["menu_item.cshtml", selectedMenuItem];
      };

      Post["/menu-item/{id}"] = parameters => {
      Ingredient newIngredient = new Ingredient(Request.Form["ingredient-name"], parameters.id);
      newIngredient.Save();
      var selectedMenuItem = MenuItem.Find(parameters.id);
      return View["menu_item.cshtml", selectedMenuItem];
      };
    }
  }
}
