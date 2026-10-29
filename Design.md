# AI acknowledgement
AI acknowledgement: I did not use generative AI in the preparation of this assignment.

# Design

Complete this document for **Part B**.

## 1. Saved recipe collection
hashset<int> was selected for the saved/favourite feature and it is named _SavedRecipes. Add(recipeid) adds a recipe ID to the collection and hashset prevent duplicates, if the user add the same recipeid to the collection the function will return a flase. Remove is the same method: Remove(recipeid), but it is quicker than array and list, because it use hash table data and perform insert, remove, and look up in constant time O(1). 


## 2. Integration

In a short paragraph, explain how the new Part B features use the existing Part A `RecipeManager` and recipe catalogue.

## 3. Basic complexity

| Operation | Structure | Expected complexity | Reason |
| --- | --- | --- | --- |
| Lookup recipe by ID | Dictionary | Average O(1) | Hash-based key lookup. |
| Traverse cooking plan | LinkedList | O(n) | Each planned recipe may need to be visited. |
| Complete next instruction | Queue | O(1) | The item at the front is removed. |
| LINQ title/ingredient search | Recipe collection | O(n) | Each recipe may need to be inspected. |
| Check whether a recipe is saved | Your chosen collection | | Explain how your collection performs membership checks. |


