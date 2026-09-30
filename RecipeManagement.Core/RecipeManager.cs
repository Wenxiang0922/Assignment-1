using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
    private Stack<int> _RemovedCookingRecipes = new Stack<int>();
    private Queue<string> _PendingInstructions = new Queue<string>();

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

    public Stack<int> removedCookingRecipes
    {
        get {return _RemovedCookingRecipes;}  
        set { _RemovedCookingRecipes = value;}
    }

    public Queue<string> pendingInstructions
    {
        get {return _PendingInstructions;}  
        set { _PendingInstructions = value;}
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
    public int CookingPlanCount => _CookingPlan.Count;
    public int PendingInstructionCount => _PendingInstructions.Count;
    public int RemovedRecipeCount => _RemovedCookingRecipes.Count;

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
            // Add the removed ID to the top of the stack list
            _RemovedCookingRecipes.Push(recipeId);
            _CookingPlan.Remove(recipeId);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RestoreLastRemovedRecipe()
    {   
        // check if the stack list is empty
        if (_RemovedCookingRecipes.Count > 0)
        {
            _CookingPlan.AddLast(_RemovedCookingRecipes.Pop());
            return true;
        }
        else
        {
            return false;
        }
    }

    public int? PeekLastRemovedRecipe()
    {
        // check if the stack list is empty
        if (_RemovedCookingRecipes.Count > 0)
        {
            return _RemovedCookingRecipes.Peek();
        }
        else
        {
            return null;
        }
    }

    public IReadOnlyList<int> GetCookingPlan()
    {
        return _CookingPlan.ToList();
    }

    public bool StartCooking(int recipeId)
    {
        // Check if id is valid
        if (_recipe.ContainsKey(recipeId))
        {   
            // Add each step of the instriction to the queue
            foreach (string instruction in _recipe[recipeId].Instructions)
            {
                _PendingInstructions.Enqueue(instruction);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public string? PeekNextInstruction()
    {   
        // check if the queue is empty
        if (_PendingInstructions.Count > 0)
        {
            return _PendingInstructions.Peek();
        }
        return null;
    }

    public string? CompleteNextInstruction()
    {   
        // requeue the oldest item if the queue list is not empty
        if (_PendingInstructions.TryDequeue(out string? instruction))
        {
            return instruction;
        }
        return null;
    }

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
