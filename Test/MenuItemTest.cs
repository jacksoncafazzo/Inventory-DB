using Xunit;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace Inventory
{
  public class MenuItemTest : IDisposable
  {
    public MenuItemTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=inventory2_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_GetIngredients_RetrievesAllTasksWithMenuItem()
    {
      MenuItem testMenuItem = new MenuItem("Spaghetti");
      testMenuItem.Save();

      Ingredient firstIngredient = new Ingredient("Noodles", testMenuItem.GetId());
      firstIngredient.Save();
      Ingredient secondIngredient = new Ingredient("Tomato", testMenuItem.GetId());
      secondIngredient.Save();


      List<Ingredient> testIngredientList = new List<Ingredient> {firstIngredient, secondIngredient};
      List<Ingredient> resultIngredientList = testMenuItem.GetIngredients();

      Assert.Equal(testIngredientList, resultIngredientList);
    }

    [Fact]
    public void Test_CategoriesEmptyAtFirst()
    {
      //Arrange, Act
      int result = MenuItem.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      MenuItem firstMenuItem = new MenuItem("Household chores");
      MenuItem secondMenuItem = new MenuItem("Household chores");

      //Assert
      Assert.Equal(firstMenuItem, secondMenuItem);
    }

    [Fact]
    public void Test_Save_SavesMenuItemToDatabase()
    {
      //Arrange
      MenuItem testMenuItem = new MenuItem("Household chores");
      testMenuItem.Save();

      //Act
      List<MenuItem> result = MenuItem.GetAll();
      List<MenuItem> testList = new List<MenuItem>{testMenuItem};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToMenuItemObject()
    {
      //Arrange
      MenuItem testMenuItem = new MenuItem("Household chores");
      testMenuItem.Save();

      //Act
      MenuItem savedMenuItem = MenuItem.GetAll()[0];

      int result = savedMenuItem.GetId();
      int testId = testMenuItem.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsMenuItemInDatabase()
    {
      //Arrange
      MenuItem testMenuItem = new MenuItem("Household chores");
      testMenuItem.Save();

      //Act
      MenuItem foundMenuItem = MenuItem.Find(testMenuItem.GetId());

      //Assert
      Assert.Equal(testMenuItem, foundMenuItem);
    }

    [Fact]
    public void Test_EqualOverrideTrueForSameDescription()
    {
      //Arrange, Act
      Ingredient firstIngredient = new Ingredient("Mow the lawn", 1);
      Ingredient secondIngredient = new Ingredient("Mow the lawn", 1);

      //Assert
      Assert.Equal(firstIngredient, secondIngredient);
    }

    [Fact]
    public void Test_Save()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("Mow the lawn", 1);
      testIngredient.Save();

      //Act
      List<Ingredient> result = Ingredient.GetAll();
      List<Ingredient> testList = new List<Ingredient>{testIngredient};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_SaveAssignsIdToObject()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("Mow the lawn", 1);
      testIngredient.Save();

      //Act
      Ingredient savedIngredient = Ingredient.GetAll()[0];

      int result = savedIngredient.GetId();
      int testId = testIngredient.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_FindFindsIngredientInDatabase()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("Mow the lawn", 1);
      testIngredient.Save();

      //Act
      Ingredient foundIngredient = Ingredient.Find(testIngredient.GetId());

      //Assert
      Assert.Equal(testIngredient, foundIngredient);
    }

    public void Dispose()
    {
      Ingredient.DeleteAll();
      MenuItem.DeleteAll();
    }
  }
}
