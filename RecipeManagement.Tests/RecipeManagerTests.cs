using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using RecipeManagement.Core;

namespace RecipeManagement.Tests;

/// <summary>
/// Example tests from the assignment specification. Add your own tests as you work.
/// </summary>
public sealed class RecipeManagerTests
{
    // my own tests PartA Phase 1
    [Fact]
    public void Constructor_NullRecipe()
    {
        Assert.Throws<ArgumentNullException>(
        () => new RecipeManager(null!));
    }
 
    [Fact]
    public void Constructor_InvalidRecipeID()
    {
        var manager =CreateManager();
        var recipe1 =new Recipe
        {
            Id = 10,
            Title = "recipe A"
        };
        var recipe2 =new Recipe
        {
            Id = 10,
            Title = "recipe b"
        };
        Assert.Throws<ArgumentException>(
        () => new RecipeManager(new[] { recipe1, recipe2 }));
    }
    [Fact]
    public void AddRecipeThatIsIDIsValid()
    {
        var manager =CreateManager();
        var recipe =new Recipe
        {
            Id = 30,
            Title = "recipe C"
        };
        Assert.True(manager.AddRecipe(recipe));
        Assert.Equal(3,manager.RecipeCount);
        Assert.Equal("recipe C",manager.FindRecipe(30)?.Title);
    }

    [Fact]
    public void AddRecipeRejected()
    {
        var manager =CreateManager();
        var recipe =new Recipe
        {
            Id = 10,
            Title = "recipe C"
        }; 
        Assert.False(manager.AddRecipe(recipe));
        Assert.Equal("Recipe A", manager.FindRecipe(10)?.Title);
    }

    [Fact]
    public void FindRecipeWithId()
    {
        var manager =CreateManager();
        Assert.Equal("Recipe A", manager.FindRecipe(10)?.Title); 
    }

    [Fact]
    public void RemoveAnRecipe()
    {
        var manager =CreateManager();
        Assert.True(manager.RemoveRecipe(10));
        Assert.Equal(0,manager.RemovedRecipeCount);
        Assert.Equal(1,manager.RecipeCount);
    }

    [Fact]
    public void UnableToRemoveAnUnIdenifiedRecipe()
    {
        var manager = CreateManager();
        Assert.False(manager.RemoveRecipe(9999));
        Assert.Equal(0,manager.RemovedRecipeCount);
        Assert.Equal(2,manager.RecipeCount);
    }

    [Fact]
    public void AddIngredients()
    {
        var manager = CreateManager();
        int conut = manager.AddIngredientsToShoppingList(10);

        var shoppingList = manager.GetShoppingList();


        Assert.Equal(1,conut);
        Assert.Equal("1 apple",manager.shoppingList[0]);
    }

    [Fact]
    public void EmptyShoppingList()
    {
        var manager = CreateManager();
        manager.AddIngredientsToShoppingList(10);
        manager.ClearShoppingList();
        int conut = manager.shoppingList.Count;

        Assert.Equal(0,conut);
    }

    // PartA Phase 2
    [Fact]
    public void AddRecipeToCookingPlan_WithValidRecipe()
    {
        var manager = CreateManager();  

        Assert.True(manager.AddRecipeToCookingPlan(10));
    }

    [Fact]
    public void AddRecipeToCookingPlan_WithInValidRecipe()
    {
        var manager = CreateManager();  

        Assert.False(manager.AddRecipeToCookingPlan(30));
    }

    [Fact]
    public void AddRecipeToCookingPlan_DuplicateRecipe()
    {
        var manager = CreateManager();  
        Assert.True(manager.AddRecipeToCookingPlan(10));
        Assert.False(manager.AddRecipeToCookingPlan(10));
    }

    [Fact]
    public void RemoveRecipeFromCookingPlan_WithValidRecipe()
    {
        var manager = CreateManager(); 

        Assert.True(manager.AddRecipeToCookingPlan(10));
        Assert.True(manager.RemoveRecipeFromCookingPlan(10));
        Assert.Equal(10,manager.PeekLastRemovedRecipe());
    }

    [Fact]
    public void RemoveRecipeFromCookingPlan_InValidRecipe()
    {
        var manager = CreateManager();  
        Assert.False(manager.RemoveRecipeFromCookingPlan(10));
    }

    [Fact]
    public void RestoreLastRemovedRecipe_NotEmpty()
    {
        var manager = CreateManager(); 

        Assert.True(manager.AddRecipeToCookingPlan(10));
        Assert.True(manager.RemoveRecipeFromCookingPlan(10));

        Assert.True(manager.RestoreLastRemovedRecipe());
        Assert.Equal(1, manager.cookingPlan.Count);
    }

    [Fact]
    public void RestoreLastRemovedRecipe_IsEmpty()
    {
        var manager = CreateManager(); 
        int cookingPlanSize = manager.cookingPlan.Count;

        Assert.False(manager.RestoreLastRemovedRecipe());
        Assert.Equal(0, cookingPlanSize);
    }

    [Fact]
    public void PeekLastRemovedRecipe_NotEmpty()
    {
        var manager = CreateManager(); 
        Assert.True(manager.AddRecipeToCookingPlan(10));
        Assert.True(manager.RemoveRecipeFromCookingPlan(10));

        Assert.Equal(10,manager.PeekLastRemovedRecipe());
    }

    [Fact]
    public void PeekLastRemovedRecipe_Empty()
    {
        var manager = CreateManager(); 
        Assert.False(manager.RemoveRecipeFromCookingPlan(10));

        Assert.Null(manager.PeekLastRemovedRecipe());
    }

    [Fact]
    public void StartCooking_validRecipe()
    {
        var manager = CreateManager(); 

        Assert.True(manager.StartCooking(10));
        Assert.Equal(2,manager.pendingInstructions.Count);
    }

    [Fact]
    public void StartCooking_InvalidRecipe()
    {
        var manager = CreateManager(); 

        Assert.False(manager.StartCooking(30));
    }

    [Fact]
    public void PeekNextInstruction_NotEmpty()
    {
        var manager = CreateManager(); 
        manager.StartCooking(10);
        manager.CompleteNextInstruction();
        Assert.Equal("Second step",manager.PeekNextInstruction());
    }

    [Fact]
    public void PeekNextInstruction_IsEmpty()
    {
        var manager = CreateManager(); 
        manager.StartCooking(10);
        manager.CompleteNextInstruction();
        manager.CompleteNextInstruction();
        Assert.Null(manager.PeekNextInstruction());
    }
    

    // Provided tests
    [Fact]
    public void Constructor_BuildsRecipeDictionary()
    {
        var manager = CreateManager();
        Assert.Equal(2, manager.RecipeCount);
        Assert.Equal("Recipe A", manager.FindRecipe(10)?.Title);
    }

    [Fact]
    public void InstructionsAreCompletedInFileOrder()
    {
        var manager = CreateManager();
        Assert.True(manager.StartCooking(10));
        Assert.Equal("First step", manager.PeekNextInstruction());
        Assert.Equal("First step", manager.CompleteNextInstruction());
        Assert.Equal("Second step", manager.PeekNextInstruction());
    }

    [Fact]
    public void RemovedRecipesAreRestoredLastInFirstOut()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);
        manager.RemoveRecipeFromCookingPlan(10);
        manager.RemoveRecipeFromCookingPlan(20);
        Assert.Equal(20, manager.PeekLastRemovedRecipe());
        Assert.True(manager.RestoreLastRemovedRecipe());
        Assert.Equal(new[] { 20 }, manager.GetCookingPlan());
    }

    private static RecipeManager CreateManager()
    {
        return new RecipeManager(new[]
        {
            new Recipe
            {
                Id = 10,
                Title = "Recipe A",
                Ingredients = new() { "1 apple" },
                Instructions = new() { "First step", "Second step" },
            },
            new Recipe
            {
                Id = 20,
                Title = "Recipe B"
            }
        });
    }

    // Part B tests
    // SearchByTitle
    [Fact]
    public void SearchExactTitle()
    {
        var manager = CreateManager();

        var result = manager.SearchByTitle("Recipe A");
        Assert.Single(result);
        Assert.Equal("Recipe A",result[0].Title);

    }

    [Fact]
    public void SearchTitleNoneCaseSensitive()
    {
        var manager = CreateManager();

        var result = manager.SearchByTitle("RECIPE b");
        Assert.Single(result);
        Assert.Equal("Recipe B",result[0].Title);

    }

    [Fact] 
    public void SearchTitleNotExist()
    {
        var manager = CreateManager();

        var result = manager.SearchByTitle("Sth that doesnt exit");
        Assert.Empty(result);  
    }

    // SearchByIngredient
    [Fact]
    public void SearchExactIngredient()
    {
        var manager = CreateManager();

        var result = manager.SearchByIngredient("apple");
        Assert.Single(result);
        Assert.Equal("Recipe A",result[0].Title);

    }

    [Fact]
    public void SearchIngredientNoneCaseSensitive()
    {
        var manager = CreateManager();

        var result = manager.SearchByIngredient("APPLE");
        Assert.Single(result);
        Assert.Equal("Recipe A",result[0].Title);

    }

    [Fact] 
    public void SearchIngredientNotExist()
    {
        var manager = CreateManager();

        var result = manager.SearchByIngredient("Ingredient that doesnt exit");
        Assert.Empty(result);  
    }
    
    // GetHighestProteinRecipes
    [Fact] 
    public void GetHighestProteinRecipesReturnsEmpty()
    {
        var manager = CreateManager();

        var result = manager.GetHighestProteinRecipes(0);
        Assert.Empty(result);  
    }
    [Fact] 
    public void GetHighestProteinRecipesNegetiveCounts()
    {
        var manager = CreateManager();

        var result = manager.GetHighestProteinRecipes(-1);
        Assert.Empty(result);  
    }

    // AddSavedRecipe
    [Fact] 
    public void AddRecipeWithvalidID()
    {
        var manager = CreateManager();

        var result = manager.AddSavedRecipe(10);
        Assert.True(result);  
    }

    [Fact] 
    public void AddRecipeWithInvalidID()
    {
        var manager = CreateManager();

        var result = manager.AddSavedRecipe(999);
        Assert.False(result);  
    }

    [Fact] 
    public void AddRecipeWithDuplicatedID()
    {
        var manager = CreateManager();
        manager.AddSavedRecipe(10);

        var result = manager.AddSavedRecipe(10);
        Assert.False(result);  
    }

    // RemoveSavedRecipe
    [Fact] 
    public void RemoveRecipeWithInvalidID()
    {
        var manager = CreateManager();

        var result = manager.RemoveSavedRecipe(10);
        Assert.False(result);  
    }

    [Fact] 
    public void RemoveRecipeWithValidID()
    {
        var manager = CreateManager();
        manager.AddSavedRecipe(10);

        var result = manager.RemoveSavedRecipe(10);
        Assert.True(result); 
    }

    // IsRecipeSaved
    [Fact] 
    public void IsRecipeSavedTrue()
    {
        var manager = CreateManager();
        manager.AddSavedRecipe(10);

        var result = manager.IsRecipeSaved(10);
        Assert.True(result); 
    }

    [Fact] 
    public void IsRecipeSavedFalse()
    {
        var manager = CreateManager();

        var result = manager.IsRecipeSaved(10);
        Assert.False(result); 
    }
    
}
