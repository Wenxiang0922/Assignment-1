using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using System.Linq;
using System.Collections;
using System.Threading.Tasks.Dataflow;

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
    private HashSet<int> _SavedRecipes = new HashSet<int>();

    //Properties
    public List<string> shoppingList
    {
        get {return _ShoppingList;}  
    }

    public LinkedList<int> cookingPlan
    {
        get {return _CookingPlan;}  
    }

    public Queue<string> pendingInstructions
    {
        get {return _PendingInstructions;}  
    }

    public RecipeManager(IEnumerable<Recipe> recipes)
    {
        // TODO Part A: validate recipes and build Dictionary<int, Recipe>.
        if (recipes == null)
        {
            throw new ArgumentNullException(nameof(recipes));
        }

        foreach (Recipe r in recipes)
        {
            if (r == null)
            {
                throw new ArgumentException(
                    "A recipe can not be empty",
                    nameof(recipes));
            }

            if (r.Id <= 0)
            {
                throw new ArgumentException(
                    "The recipe ID must be greater than 0",
                    nameof(recipes));
            }

            if (_recipe.ContainsKey(r.Id))
            {
                throw new ArgumentException(
                    $"This ID ${r.Id} alredy exit",
                    nameof(recipes));
            }

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
        if(recipe == null)
        {
            throw new ArgumentNullException(
                "A recipe can not be empty",
                nameof(recipe)
            );
        }
        if (recipe.Id <= 0)
        {
            throw new ArgumentException(
                "The recipe ID have to be greater than 0",
                nameof(recipe)
            );
        }

        else
        {   
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

    public IReadOnlyList<Recipe> SearchByTitle(string searchText)
    {
        return _recipe.Values
        .Where(r => 
        r.Title.Contains(searchText,StringComparison.OrdinalIgnoreCase))
        .ToList();
    }

    public IReadOnlyList<Recipe> SearchByIngredient(string searchText)
    {
        return _recipe.Values
        .Where(r => r.Ingredients
        .Any(I => I.Contains(searchText,StringComparison.OrdinalIgnoreCase)))
        .ToList();
    }

    public IReadOnlyList<Recipe> GetHighestProteinRecipes(int count)
    {   
        if (count == null || count<=0 ) 
        {
            return new List<Recipe>() ;
        }
        
        return _recipe.Values.Where(r => r.Nutrition !=null)
        .OrderByDescending(r => r.Nutrition.ProteinG).Take(count).ToList();
         
    }

    public bool AddSavedRecipe(int recipeId)
    {   
        if (!_recipe.ContainsKey(recipeId))
        {   
            Console.WriteLine("Invalide recipe ID");
            return false; 
        }
        if (_SavedRecipes.Contains(recipeId))
        {
            Console.WriteLine("The recipe is already saved");
            return false;
        }
        _SavedRecipes.Add(recipeId);
        return true;
    }

    public bool RemoveSavedRecipe(int recipeId)
    {
        if (_SavedRecipes.Contains(recipeId))
        {
            _SavedRecipes.Remove(recipeId);
            return true;
        }
        else
        {
            Console.WriteLine("This recipe ID was not found in your saved list");
            return false;
        }
    }

    public bool IsRecipeSaved(int recipeId)
    {   
        if (_SavedRecipes.Contains(recipeId))
        {   
            return true;
        }
        else if (_SavedRecipes.Count== 0)
        {   
            Console.WriteLine("The save list is empty");
            return false;
        }
        else
        {   
            return false;
        }
    }

    public IReadOnlyList<int> GetSavedRecipes()
    {
        return _SavedRecipes.ToList();
    }
}
