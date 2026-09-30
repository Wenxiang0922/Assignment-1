using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

namespace RecipeManagement.Core;

/// <summary>
/// Implement this class using the five Part A collections as private fields:
/// Dictionary&lt;int, Recipe&gt;, List&lt;string&gt;, LinkedList&lt;int&gt;,
/// Stack&lt;int&gt; and Queue&lt;string&gt;.
/// </summary>
public sealed class RecipeManager : IRecipeManager
{
    // TODO Part A: add your private collection fields here.
    private Dictionary<int,Recipe> _recipe = new Dictionary<int, Recipe>();
    private List<string> _ShoppingList = new List<string>();
    private LinkedList<int> _CookingPlan = new LinkedList<int>();
   


    //Properties
    public Dictionary<int, Recipe> recipe 
    {
        get {return _recipe;}  
        set { _recipe = value;}
    }
    public List<string> shoppingList
    {
        get {return _ShoppingList;}  
        set { _ShoppingList = value;}
    }

    public LinkedList<int> cookingPlan
    {
        get {return _CookingPlan;}  
        set { _CookingPlan = value;}
    }

    public RecipeManager(IEnumerable<Recipe> recipes)
    {
        // TODO Part A: validate recipes and build Dictionary<int, Recipe>.
        _ = recipes;
        foreach (Recipe r in recipes)
        {
            _recipe.Add(r.Id,r);
        }
    }

    public int RecipeCount => _recipe.Count;
    public int ShoppingItemCount => _ShoppingList.Count;
    public int CookingPlanCount => 0;
    public int PendingInstructionCount => 0;
    public int RemovedRecipeCount => 0;

    public bool AddRecipe(Recipe recipe)
    {   
        // Don take the recipe if the recipe is already in the _recipe dictionary
        if (_recipe.ContainsKey(recipe.Id))
        {
            return false;
        }
        else
        {   
            // add a recipe that have a valid id 
            return _recipe.TryAdd(recipe.Id,recipe);
        }
    }

    public Recipe? FindRecipe(int recipeId)
    {
        return _recipe[recipeId];
    }

    public bool RemoveRecipe(int recipeId) 
    {   
        // remove the object if the recipeId was found in the list
        if (_recipe.ContainsKey(recipeId))
        {   
            return _recipe.Remove(recipeId);
        }
        else
        {
            return false;
        }

    }

    public int AddIngredientsToShoppingList(int recipeId)
    {   
        // add every ingredients for that recipe in order
        foreach (var items in _recipe[recipeId].Ingredients)
        {
            _ShoppingList.Add(items);
        }
        return _ShoppingList.Count;
    }

    public IReadOnlyList<string> GetShoppingList()
    {
        return _ShoppingList.AsReadOnly();
    }

    public void ClearShoppingList()
    {
        _ShoppingList.Clear();
    }

    public bool AddRecipeToCookingPlan(int recipeId)
    {    
        // Check if id is valid
        if (_recipe.ContainsKey(recipeId))
        {   
            // reject repetetive cooking plan
            if (_CookingPlan.Contains(recipeId))
            {
                return false;
            }
            else
            {
                _CookingPlan.AddLast(recipeId);
                return true;  
            }
        }
        else
        {
            return false;
        }
    }

    public bool RemoveRecipeFromCookingPlan(int recipeId)
    {   
        // Check if the _CookingPlan have recipe
        if (_CookingPlan.Contains(recipeId))
        {   
            _CookingPlan.Remove(recipeId);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RestoreLastRemovedRecipe() =>
        throw new NotImplementedException("Part A: implement RestoreLastRemovedRecipe.");

    public int? PeekLastRemovedRecipe() =>
        throw new NotImplementedException("Part A: implement PeekLastRemovedRecipe.");

    public IReadOnlyList<int> GetCookingPlan()
    {
        return _CookingPlan.ToList();
    }

    public bool StartCooking(int recipeId) =>
        throw new NotImplementedException("Part A: implement StartCooking.");

    public string? PeekNextInstruction() =>
        throw new NotImplementedException("Part A: implement PeekNextInstruction.");

    public string? CompleteNextInstruction() =>
        throw new NotImplementedException("Part A: implement CompleteNextInstruction.");

    public IReadOnlyList<Recipe> SearchByTitle(string searchText) =>
        throw new NotImplementedException("Part B: implement SearchByTitle.");

    public IReadOnlyList<Recipe> SearchByIngredient(string searchText) =>
        throw new NotImplementedException("Part B: implement SearchByIngredient.");

    public IReadOnlyList<Recipe> GetHighestProteinRecipes(int count) =>
        throw new NotImplementedException("Part B: implement GetHighestProteinRecipes.");

    public bool AddSavedRecipe(int recipeId) =>
        throw new NotImplementedException("Part B: implement AddSavedRecipe.");

    public bool RemoveSavedRecipe(int recipeId) =>
        throw new NotImplementedException("Part B: implement RemoveSavedRecipe.");

    public bool IsRecipeSaved(int recipeId) =>
        throw new NotImplementedException("Part B: implement IsRecipeSaved.");

    public IReadOnlyList<int> GetSavedRecipes() =>
        throw new NotImplementedException("Part B: implement GetSavedRecipes.");
}
