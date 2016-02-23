using Xunit;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace Inventory
{
  public class IngredientTest : IDisposable
  {

    public IngredientTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=inventory2_test;Integrated Security=SSPI;";
    }

        [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Ingredient.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

        [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      Ingredient firstIngredient = new Ingredient("Mow the lawn", 1);
      Ingredient secondIngredient = new Ingredient("Mow the lawn", 1);

      //Assert
      Assert.Equal(firstIngredient, secondIngredient);
    }

        [Fact]
    public void Test_Find_FindsIngredientInDatabase()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("Mow the lawn", 1);
      testIngredient.Save();

      //Act
      Ingredient foundIngredient = Ingredient.Find(testIngredient.GetId());

      //Assert
      Assert.Equal(testIngredient, foundIngredient);
    }

        [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("Mow the lawn", 1);

      //Act
      testIngredient.Save();
      List<Ingredient> result = Ingredient.GetAll();
      List<Ingredient> testList = new List<Ingredient>{testIngredient};

      //Assert
      Assert.Equal(testList, result);
    }

    public void Dispose()
    {
      Ingredient.DeleteAll();
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
  }
}
