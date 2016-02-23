using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System;

namespace Inventory
{
  public class MenuItem
  {
    private int _id;
    private string _name;

    public MenuItem (string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public override bool Equals(System.Object otherMenuItem)
    {
        if (!(otherMenuItem is MenuItem))
        {
          return false;
        }
        else
        {
          MenuItem newMenuItem = (MenuItem) otherMenuItem;
          bool idEquality = this.GetId() == newMenuItem.GetId();
          bool nameEquality = this.GetName() == newMenuItem.GetName();
          return (idEquality && nameEquality);
        }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public static List<MenuItem> GetAll()
    {
      List<MenuItem> allMenuItems = new List<MenuItem>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM menu_items;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int MenuItemId = rdr.GetInt32(0);
        string MenuItemName = rdr.GetString(1);
        MenuItem newMenuItem = new MenuItem(MenuItemName, MenuItemId);
        allMenuItems.Add(newMenuItem);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allMenuItems;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO menu_items (name) OUTPUT INSERTED.id VALUES (@MenuItemName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@MenuItemName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM menu_items;", conn);
      cmd.ExecuteNonQuery();
    }

    public static MenuItem Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM menu_items WHERE id = @MenuItemId;", conn);
      SqlParameter MenuItemIdParameter = new SqlParameter();
      MenuItemIdParameter.ParameterName = "@MenuItemId";
      MenuItemIdParameter.Value = id.ToString();
      cmd.Parameters.Add(MenuItemIdParameter);
      rdr = cmd.ExecuteReader();

      int foundMenuItemId = 0;
      string foundMenuItemDescription = null;

      while(rdr.Read())
      {
        foundMenuItemId = rdr.GetInt32(0);
        foundMenuItemDescription = rdr.GetString(1);
      }
      MenuItem foundMenuItem = new MenuItem(foundMenuItemDescription, foundMenuItemId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundMenuItem;
    }

    public List<Ingredient> GetIngredients()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients WHERE menu_item_id = @MenuItemId;", conn);
      SqlParameter MenuItemIdParameter = new SqlParameter();
      MenuItemIdParameter.ParameterName = "@MenuItemId";
      MenuItemIdParameter.Value = this.GetId();
      cmd.Parameters.Add(MenuItemIdParameter);
      rdr = cmd.ExecuteReader();

      List<Ingredient> ingredients  = new List<Ingredient> {};
      while(rdr.Read())
      {
        int IngredientId = rdr.GetInt32(0);
        string IngredientDescription = rdr.GetString(1);
        int IngredientCategoryId = rdr.GetInt32(2);
        Ingredient newIngredient = new Ingredient(IngredientDescription, IngredientCategoryId, IngredientId);
        ingredients.Add(newIngredient);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return ingredients;
    }
  }
}
