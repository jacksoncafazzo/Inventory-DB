using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System;

namespace Inventory
{

    public class Ingredient
{
  private int _id;
  private string _description;
  private int _menuItemId;

  public Ingredient(string Description, int MenuItemId, int Id = 0)
  {
    _id = Id;
    _description = Description;
    _menuItemId = MenuItemId;
  }

  public override bool Equals(System.Object otherIngredient)
  {
      if (!(otherIngredient is Ingredient))
      {
        return false;
      }
      else
      {
        Ingredient newIngredient = (Ingredient) otherIngredient;
        bool idEquality = this.GetId() == newIngredient.GetId();
        bool descriptionEquality = this.GetDescription() == newIngredient.GetDescription();
        bool MenuItemEquality = this.GetMenuItemId() == newIngredient.GetMenuItemId();
        return (idEquality && descriptionEquality && MenuItemEquality);
      }
  }

  public static void DeleteAll()
{
  SqlConnection conn = DB.Connection();
  conn.Open();
  SqlCommand cmd = new SqlCommand("DELETE FROM ingredients;", conn);
  cmd.ExecuteNonQuery();
}

    public int GetId()
    {
      return _id;
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public int GetMenuItemId()
  {
    return _menuItemId;
  }
  public void SetMenuItemId(int newMenuItemId)
  {
    _menuItemId = newMenuItemId;
  }
  public static List<Ingredient> GetAll()
  {
    List<Ingredient> AllIngredients = new List<Ingredient>{};

    SqlConnection conn = DB.Connection();
    SqlDataReader rdr = null;
    conn.Open();

    SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients;", conn);
    rdr = cmd.ExecuteReader();

    while(rdr.Read())
    {
      int ingredientId = rdr.GetInt32(0);
      string ingredientDescription = rdr.GetString(1);
      int ingredientCategoryId = rdr.GetInt32(2);
      Ingredient newIngredient = new Ingredient(ingredientDescription, ingredientCategoryId, ingredientId);
      AllIngredients.Add(newIngredient);
    }
    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }
    return AllIngredients;
  }
  public void Save()
  {
    SqlConnection conn = DB.Connection();
    SqlDataReader rdr;
    conn.Open();

    SqlCommand cmd = new SqlCommand("INSERT INTO ingredients (description, menu_item_id) OUTPUT INSERTED.id VALUES (@IngredientDescription, @IngredientCategoryId);", conn);

    SqlParameter descriptionParameter = new SqlParameter();
    descriptionParameter.ParameterName = "@IngredientDescription";
    descriptionParameter.Value = this.GetDescription();

    SqlParameter MenuItemIdParameter = new SqlParameter();
    MenuItemIdParameter.ParameterName = "@IngredientCategoryId";
    MenuItemIdParameter.Value = this.GetMenuItemId();

    cmd.Parameters.Add(descriptionParameter);
    cmd.Parameters.Add(MenuItemIdParameter);

    rdr = cmd.ExecuteReader();

    while(rdr.Read())
    {
      this._id = rdr.GetInt32(0);
    }
    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }
  }

  public static Ingredient Find(int id)
  {
    SqlConnection conn = DB.Connection();
    SqlDataReader rdr = null;
    conn.Open();

    SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients WHERE id = @IngredientId;", conn);
    SqlParameter ingredientIdParameter = new SqlParameter();
    ingredientIdParameter.ParameterName = "@IngredientId";
    ingredientIdParameter.Value = id.ToString();
    cmd.Parameters.Add(ingredientIdParameter);
    rdr = cmd.ExecuteReader();

    int foundIngredientId = 0;
    string foundIngredientDescription = null;
    int foundIngredientCategoryId = 0;

    while(rdr.Read())
    {
      foundIngredientId = rdr.GetInt32(0);
      foundIngredientDescription = rdr.GetString(1);
      foundIngredientCategoryId = rdr.GetInt32(2);
    }
    Ingredient foundIngredient = new Ingredient(foundIngredientDescription, foundIngredientCategoryId, foundIngredientId);

    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }
    return foundIngredient;
  }
}
}
