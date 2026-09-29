using System.Collections.Generic;
using RecipeManagement.Core;

namespace RecipeManagement.Tests;

/// <summary>
/// Example tests from the assignment specification. Add your own tests as you work.
/// </summary>
public sealed class RecipeManagerTests
{
    // my own tests
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
        Assert.Equal(1,manager.RecipeCount);
    }

        [Fact]
    public void UnableToRemoveAnUnIdenifiedRecipe()
    {
        var manager = CreateManager();
        Assert.False(manager.RemoveRecipe(9999));
        Assert.Equal(2,manager.RecipeCount);
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
                Instructions = new() { "First step", "Second step" }
            },
            new Recipe
            {
                Id = 20,
                Title = "Recipe B"
            }
        });
    }

}
